using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBio : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI BDText; 
    public TextMeshProUGUI OrientText;
    public TextMeshProUGUI HehimText;
    public TextMeshProUGUI ChortText;
    public TextMeshProUGUI WeightText;
    public TextMeshProUGUI HieghtText;
    public TextMeshProUGUI EdicyText;
    public TextMeshProUGUI OpitText;
    public TextMeshProUGUI Chort1Text;
    public TextMeshProUGUI Chort2Text;


    void Start()
    {
        
    }


    void Update()
    {
        PersonManager instance = PersonManager.Instance;
        PersonIdentety person = PersonManager.Instance.persons[0];
        var stats = person.GetModificator<BasicStats>();
        if (Input.GetKeyDown (KeyCode.F)) 
        {
            NameText.text = stats.name;
            BDText.text = stats.age.ToString();
            OrientText.text = stats.orientation.ToString();
           /* HehimText.text = stats.sex.ToString();
            ChortText.text = stats.status.ToString();*/
            WeightText.text = stats.weight.ToString();
            HieghtText.text = stats.height.ToString();
            EdicyText.text = stats.education.ToString();
            OpitText.text = "1(Не доделано)";
            Chort1Text.text = "Хуево играет в доту";
            Chort2Text.text = "СДВГ ПРОГЕР УЕБОК";
        }
        
    }

}
/*   public string name = "-";
    public PersonSex sex;
    public PersonStatus status;
    public PersonEducation education;
    public PersonOrientation orientation;
    public float height;
    public int age;
    public int weight;
 */ 
  