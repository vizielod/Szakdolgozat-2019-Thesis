using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    static CharacterMovement _instance;
    static PluginWrapper pw_instance;

    static PlayerRotation pr_instance;
    


    public float rotationSpeed = 5f;

    private float x;
    private float y;
    private float z;

    private Vector3 inputRotation;
    private Rigidbody rb_character;

    public static CharacterMovement GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("CharacterMovement").AddComponent<CharacterMovement>();
        }
        return _instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        pw_instance = PluginWrapper.GetInstance();
        //pr_instance = PlayerRotation.GetInstance(); //Used for testing GetInstance()
        x = pw_instance.getAngleX();
        y = pw_instance.getAngleY();
        z = pw_instance.getAngleZ();
        Debug.Log("X coord: " + x + ", Y coord: " + y + ", Z coord: " + z);
        rb_character = GetComponent<Rigidbody>();
        Debug.Log(rb_character);
        //Debug.Log(rb_character);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pr_instance.GetPosition()); //part of the pr_instance GetInstance() test
    }

    private void FixedUpdate()
    {
        x = pw_instance.getAngleX();
        y = pw_instance.getAngleY();
        z = pw_instance.getAngleZ();
        inputRotation = new Vector3(x, -y, -z);
        //Quaternion deltaRotation = Quaternion.Euler(inputRotation * Time.deltaTime * rotationSpeed);
        //rb_character.MoveRotation(rb_character.rotation * deltaRotation);
        rb_character.transform.Rotate(inputRotation * Time.deltaTime);

    }
}
