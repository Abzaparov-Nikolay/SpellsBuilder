using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody body;
    float threesectimer = 0f;
    [SerializeField]
    private float speed;
    private float InventorySpeed = 1;

    [SerializeField] private AnimationStateSetter stateSetter;

    private bool rotateToMouse = false;

    [SerializeField] private ClientNetworkTransform1 cnt;

    //NETCODE
    [SerializeField] private float reconciliationCooldownTime = 1f;
    //NetworkTimer networkTimer;
    float serverTickRate = 30;
    const int bufferSize = 1024;

    //CLIENT NETCODE
    CircularBuffer<StatePayload> clientStateBuffer;
    CircularBuffer<InputPayload> clientInputBuffer;
    StatePayload lastServerState;
    StatePayload lastProcessedState;

    //Server netcode
    CircularBuffer<StatePayload> serverStateBuffer;
    Queue<InputPayload> serverInputQueue;
    CountdownTimer reconciliationCooldown;

    [SerializeField] private float reconcileThreashhold = 15f;
    [SerializeField] private float extrapolationLimit = 0.1f; //500 ms
    [SerializeField] private float extrapolationMultiplier = 1.4f;


    StatePayload extrapolationState;
    CountdownTimer extrapolationCooldown;

    private void Awake()
    {
        //NetworkManager.NetworkTickSystem.
        cnt = GetComponent<ClientNetworkTransform1>();

        serverTickRate = NetworkManager.NetworkTickSystem.TickRate;

        //networkTimer = new NetworkTimer(serverTickRate);
        clientStateBuffer = new(bufferSize);
        clientInputBuffer = new(bufferSize);

        serverStateBuffer = new(bufferSize);
        serverInputQueue = new Queue<InputPayload>();

        reconciliationCooldown = new CountdownTimer(reconciliationCooldownTime);
        extrapolationCooldown = new(extrapolationLimit);

        reconciliationCooldown.OnTimerStart += () =>
        {
            extrapolationCooldown.Stop();
        };


        extrapolationCooldown.OnTimerStart += () =>
        {
            reconciliationCooldown.Stop();
            cnt.authorityMode = AuthorityMode.Server;
            ChangeAuthoritySync(cnt, AuthorityMode.Server);
        };
        extrapolationCooldown.OnTimerStop += () =>
        {
            extrapolationState = default;
            ChangeAuthoritySync(cnt, AuthorityMode.Client);
        };
    }

    private void ChangeAuthoritySync(ClientNetworkTransform1 cnt, AuthorityMode mode)
    {
        cnt.authorityMode = mode;
        var sync = mode == AuthorityMode.Client;
        cnt.SyncPositionX = sync;
        cnt.SyncPositionY = sync;
        cnt.SyncPositionZ = sync;
        cnt.SyncRotAngleX = sync;
        cnt.SyncRotAngleY = sync;
        cnt.SyncRotAngleZ = sync;
        cnt.SyncScaleX = sync;
        cnt.SyncScaleY = sync;
        cnt.SyncScaleZ = sync;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //if (!base.IsOwner)
        //    this.enabled = false;
        MagicInputHandler.SpellCastStarting += StartRotateToMouse;
        MagicInputHandler.SpellCastStopped += StopRotationToMouse;
        NetworkManager.NetworkTickSystem.Tick += NetworkTicmerTick;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        MagicInputHandler.SpellCastStarting -= StartRotateToMouse;
        MagicInputHandler.SpellCastStopped -= StopRotationToMouse;
        NetworkManager.NetworkTickSystem.Tick -= NetworkTicmerTick;
    }


    private void NetworkTicmerTick()
    {
        HandleClientTick();
        HandleServerTick();
    }



    private void FixedUpdate()
    {
        Extrapolate();
    }

    private static float CalculateLatencyMS(InputPayload input)
    {
        return (DateTime.Now - input.timeStamp).Milliseconds;// / 1000f;
    }

    private void HandleServerTick()
    {
        if (!IsServer) return;
        var bufferIndex = -1;
        InputPayload inputPayload = default;
        while (serverInputQueue.Count > 0)
        {
            inputPayload = serverInputQueue.Dequeue();
            bufferIndex = inputPayload.tick % bufferSize;

            StatePayload state = ProcessMovement(inputPayload); //SIMULATE MOVEMENT WAS HERE
            serverStateBuffer.Add(state, bufferIndex);
        }

        if (bufferIndex == -1) return;
        SendToClientRpc(serverStateBuffer.Get(bufferIndex));
        HandleExtrapolation(serverStateBuffer.Get(bufferIndex), CalculateLatencyMS(inputPayload));
    }

    private void HandleExtrapolation(StatePayload state, float latency)
    {
        if (ShouldExtrapolate(latency))
        {
            float axisLength = latency * state.angularVelocity.magnitude * Mathf.Rad2Deg;
            Quaternion angularRotation = Quaternion.AngleAxis(axisLength, state.angularVelocity);
            if (extrapolationState.position != default)
            {
                state = extrapolationState;
            }

            var posAdjustment = state.velocity * (1 + latency * extrapolationMultiplier);
            extrapolationState.position = posAdjustment;
            extrapolationState.velocity = state.velocity;
            extrapolationState.angularVelocity = state.angularVelocity;
            extrapolationState.rotation = angularRotation * state.rotation;
            extrapolationCooldown.Start();
        }
        else
        {
            extrapolationCooldown.Stop();
        }
    }

    private bool ShouldExtrapolate(float latency) => latency < extrapolationLimit && latency > Time.fixedDeltaTime;

    void Extrapolate()
    {
        if (IsServer && extrapolationCooldown.IsRunning)
        {
            body.position += new Vector3(extrapolationState.position.x, 0, extrapolationState.position.z);
        }
    }
    //StatePayload SimulateMavoment(InputPayload input)
    //{
    //    Physics.simulationMode = SimulationMode.Script;

    //    Move(input.inputVector);
    //    Physics.Simulate(Time.fixedDeltaTime);

    //    Physics.simulationMode = SimulationMode.FixedUpdate;

    //    return new StatePayload()
    //    {
    //        tick = input.tick,
    //        position = transform.position,
    //        rotation = transform.rotation,
    //        velocity = body.velocity,
    //        angularVelocity = body.angularVelocity,
    //    };
    //}

    [ClientRpc]
    void SendToClientRpc(StatePayload state)
    {
        if (!IsOwner) return;
        lastServerState = state;
    }

    private void HandleClientTick()
    {
        if (!IsClient || !IsOwner) return;

        var currentTick = NetworkManager.NetworkTickSystem.LocalTime.Tick;
        var bufferIndex = currentTick % bufferSize;
        var inputPayload = new InputPayload()
        {
            tick = currentTick,
            timeStamp = DateTime.Now,
            networkObjectId = NetworkObject.NetworkObjectId,
            inputVector = InputDirectionProvider.Get().normalized,
            position = transform.position
            //new Vector3(InputDirectionProvider.Get().x, 0, InputDirectionProvider.Get().y)
        };

        clientInputBuffer.Add(inputPayload, bufferIndex);
        SendToServerRpc(inputPayload);

        var statePayload = ProcessMovement(inputPayload);
        clientStateBuffer.Add(statePayload, bufferIndex);

        HandleServerReconciliation();
    }

    private void HandleServerReconciliation()
    {
        if (!ShouldReconcile())
        {
            return;
        }

        float positionError;
        int bufferIndex;
        StatePayload rewindState = default;

        bufferIndex = lastServerState.tick % bufferSize;
        if (bufferIndex - 1 < 0)
        {
            return;
        }

        rewindState = IsHost ? serverStateBuffer.Get(bufferIndex - 1) : lastServerState;
        positionError = Vector3.Distance(rewindState.position, clientStateBuffer.Get(bufferIndex).position);

        if (positionError > reconcileThreashhold)
        {
            ReconcileState(rewindState);
            reconciliationCooldown.Start();
        }

        lastProcessedState = lastServerState;
    }

    private bool ShouldReconcile()
    {
        bool isNewServerState = !lastServerState.Equals(default);
        bool isLastStateUndefinedOrDifferent = lastProcessedState.Equals(default)
            || !lastProcessedState.Equals(lastServerState);

        return isNewServerState
            && isLastStateUndefinedOrDifferent
            && !reconciliationCooldown.IsRunning
            && !extrapolationCooldown.IsRunning;
    }

    private void ReconcileState(StatePayload rewindState)
    {
        transform.position = rewindState.position;
        transform.rotation = rewindState.rotation;
        body.velocity = rewindState.velocity;
        body.angularVelocity = rewindState.angularVelocity;

        if (!rewindState.Equals(lastServerState))
        {
            return;
        }

        clientStateBuffer.Add(rewindState, rewindState.tick);

        int tickToReplay = lastServerState.tick;
        while (tickToReplay < NetworkManager.NetworkTickSystem.LocalTime.Tick)
        {
            int bufferIndex = tickToReplay % bufferSize;
            StatePayload state = ProcessMovement(clientInputBuffer.Get(bufferIndex));
            clientStateBuffer.Add(state, bufferIndex);
            tickToReplay++;
        }
    }

    [ServerRpc]
    private void SendToServerRpc(InputPayload input)
    {
        serverInputQueue.Enqueue(input);
    }

    private StatePayload ProcessMovement(InputPayload input)
    {
        Move(input.inputVector);
        return new StatePayload()
        {
            tick = input.tick,
            networkObjectId = input.networkObjectId,
            position = transform.position,
            rotation = transform.rotation,
            velocity = body.velocity,
            angularVelocity = body.angularVelocity
        };
    }

    private void Move(Vector3 inputVector)
    {
        var fraction = /*Time.deltaTime /*/ 1 / NetworkManager.NetworkTickSystem.TickRate;
        //new Vector3(inputVector.x, 0, inputVector.y);
        body.velocity = new Vector3(inputVector.x, 0, inputVector.y).normalized * speed * InventorySpeed;// * fraction;
        if (rotateToMouse)
        {

            var facingDir = (InputMousePosProvider.Get() - body.position).normalized;
            var dir = new Vector3(facingDir.x, 0, facingDir.z).normalized;
            body.rotation = Quaternion.LookRotation(dir, Vector3.up);

        }

        if (body.velocity.magnitude > 0)
        {
            if (!rotateToMouse)
                body.rotation = Quaternion.LookRotation(body.velocity.normalized, Vector3.up);
            body.constraints = (RigidbodyConstraints)80;
            ChangeAnimationState(PlayerState.Walking);
        }
        else
        {
            body.constraints = (RigidbodyConstraints)112;
            ChangeAnimationState(PlayerState.Idle);

        }
    }

    private void Update()
    {
        //networkTimer.Update(Time.deltaTime);
        reconciliationCooldown.Tick(Time.deltaTime);
        extrapolationCooldown.Tick(Time.deltaTime);
        if (!IsOwner) return;
        threesectimer += Time.deltaTime;
        if (threesectimer >= 3)
        {
            threesectimer = 0;
            GetSpeedServerRpc();
        }

    }

    private void StartRotateToMouse()
    {
        rotateToMouse = true;
    }

    private void StopRotationToMouse()
    {
        rotateToMouse = false;
    }

    private void ChangeAnimationState(PlayerState state)
    {
        if (stateSetter != null)
            stateSetter.Set(state);
    }

    [ServerRpc]
    private void GetSpeedServerRpc()
    {
        UpdateSpeedOnClientClientRpc(PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.Speed));
    }

    [ClientRpc]
    private void UpdateSpeedOnClientClientRpc(float newVAlue)
    {
        InventorySpeed = newVAlue;
    }
}
