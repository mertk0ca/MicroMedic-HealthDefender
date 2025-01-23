using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour
{
    public WheelJoint2D wheelF;  //on teker referansi   
    public WheelJoint2D wheelR;  //arka teker referansi

    private JointMotor2D FrontMotor;  //on motor referansý
    private JointMotor2D RearMotor;  //arka motor referansi

    public float SpeedForward = 600f;  //ileri gitme hizi
    public float SpeedBackward = -300f;  //geri gitme hizi
    public float Tork = 1000f;  //tork degeri

    public float HorizontalInput;  //yatay input

    public float DecelerationSpeed = 2f; // gazi birakinca yavaslama hizi

    private float currentMotorSpeedF = 0f;
    private float currentMotorSpeedR = 0f;

    public AudioSource engineSound; 
    public float maxVolume = 0.5f; // maksimum ses seviyesi
    public float minPitch = 0.9f; // minimum pitch degeri
    public float maxPitch = 1f;   // maksimum pitch degeri

    private float targetVolume = 0f; // rolanti motor sesi
    private bool isEnginePlaying = false;  //motor ses cikariyor mu

    // Start is called before the first frame update
    void Start()
    {
        FrontMotor = wheelF.motor;
        RearMotor = wheelR.motor;

        if (engineSound != null)
        {
            engineSound.loop = true;  // sesi dongude calmak icin
            engineSound.volume = 0f;  // baslangicta sesi sifir yap
            engineSound.pitch = minPitch; // baslangicdaki pitch degerini minimum olarak ayarlama
        }
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        ApplyForce();
        AdjustEngineSoundVolume();
        AdjustEngineSoundPitch();
    }

    private void ApplyForce()//tekerleri dondurme fonksiyonu
    {
        if (HorizontalInput > 0)  //ileri git
        {
            currentMotorSpeedF = SpeedForward;
            currentMotorSpeedR = SpeedForward;

            // gaza basildiginda motor sesini oynat
            if (engineSound != null && !isEnginePlaying)
            {
                engineSound.Play();
                isEnginePlaying = true;
            }

            // motor sesinin volume degerini arttýr
            targetVolume = maxVolume;
        }
        else if (HorizontalInput < 0)  //geri git
        {
            currentMotorSpeedF = SpeedBackward;
            currentMotorSpeedR = SpeedBackward;

            // gaza basildiginda motor sesini oynat
            if (engineSound != null && !isEnginePlaying)
            {
                engineSound.Play();
                isEnginePlaying = true;
            }

            // motor sesinin volume degerini arttýr
            targetVolume = maxVolume;
        }
        else
        {
            // gazi biraktiginda motor hizini yavas yavas sifira cek
            currentMotorSpeedF = Mathf.Lerp(currentMotorSpeedF, 0, DecelerationSpeed * Time.deltaTime);
            currentMotorSpeedR = Mathf.Lerp(currentMotorSpeedR, 0, DecelerationSpeed * Time.deltaTime);

            // sesi azalt
            targetVolume = 0.01f;
        }

        // motor ayarlari
        FrontMotor.motorSpeed = currentMotorSpeedF;
        FrontMotor.maxMotorTorque = Tork;

        RearMotor.motorSpeed = currentMotorSpeedR;
        RearMotor.maxMotorTorque = Tork;

        wheelF.motor = FrontMotor;
        wheelR.motor = RearMotor;
    }

    private void AdjustEngineSoundVolume()
    {
        if (engineSound != null)
        {
            // motor sesini yavasca hedeflenen volume e getir
            engineSound.volume = Mathf.Lerp(engineSound.volume, targetVolume, Time.deltaTime * 2f);
        }
    }

    private void AdjustEngineSoundPitch()
    {
        if (engineSound != null)
        {
            // gaza bastikca pitch degeri artar basilmadiginda pitch degeri azalýr
            if (HorizontalInput > 0)
            {
                engineSound.pitch = Mathf.Lerp(engineSound.pitch, maxPitch, Time.deltaTime * 2f);
            }
            else if (HorizontalInput < 0)
            {
                engineSound.pitch = Mathf.Lerp(engineSound.pitch, minPitch, Time.deltaTime * 2f);
            }
            else
            {
                engineSound.pitch = Mathf.Lerp(engineSound.pitch, minPitch, Time.deltaTime * 2f);
            }
        }
    }
}
