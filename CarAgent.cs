using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class CarAgent : Agent
{
    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    public Transform target;

    private Rigidbody carRigidbody;

    public override void Initialize()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 0.1f, 0);
        transform.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.InverseTransformDirection(target.position - transform.position));
        sensor.AddObservation(carRigidbody.velocity.magnitude);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        float steer = actions.ContinuousActions[0];
        float accelerate = actions.ContinuousActions[1];

        frontLeftWheel.steerAngle = steer * 30f;
        frontRightWheel.steerAngle = steer * 30f;
        rearLeftWheel.motorTorque = accelerate * 1500f;
        rearRightWheel.motorTorque = accelerate * 1500f;

        if (Vector3.Distance(transform.position, target.position) < 1.5f)
        {
            SetReward(1.0f);
            EndEpisode(); 
        }

        if (transform.position.y < -1)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }
}