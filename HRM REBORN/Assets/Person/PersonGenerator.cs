using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

class PersonGenerator : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;
    const int requirementGW = 4;
    const int maxGW = requirementGW + 2;
    const int minGW = requirementGW - 3;

    static PersonIdentety person;
    int weight;
    static PersonGenerator Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateNewPerson()
    {
        GameObject _person = Instantiate(personPrefab);

        _person.transform.position = transform.position;

        person = _person.GetComponent<PersonIdentety>();
        weight = 0;

        PersonSex sex = PersonSex.Man;
        if (Random.Range(0, 2) == 1)
        {
            sex = PersonSex.Woman;
        }
        var stats = new BasicStats();

        stats.status = PersonStatus.hidden;
        stats.sex = sex;

        person.AddModificator(stats);

        GenerateRandomFeatures();


        if (Random.Range(0, 4) == 3)
        {
            GenerateRandomNationalFeature();
        }

        float height;
        int age;
        int personWeight;

        if (sex == PersonSex.Man)
        {
            height = Random.Range(160, 210);
        }
        else
        {
            height = Random.Range(140, 190);
        }

        if (Random.Range(0, 2) == 1)
        {
            age = GenerateRandomAge();
            personWeight = GenerateRandomWeight(height);
        }
        else
        {
            personWeight = GenerateRandomWeight(height);
            age = GenerateRandomAge();
        }

        PersonEducation education = GenerateEducation();

        stats.education = education;
        stats.age = age;
        stats.height = height / 100;
        stats.weight = weight;

        //return new PersonIdentety(age, sex, personWeight, height, PersonOrentation.hetero, education, features);
    }

    PersonEducation GenerateEducation()
    {
        int requiredW = requirementGW - weight;

        var education = educations.Where(i => i.weight == requiredW).ElementAt(0);

        weight += education.weight;
        return education.type;
    }

    int GenerateRandomAge()
    {
        int minW = minGW - weight;//-5 - 0
        int maxW = maxGW - weight;//0 - 5

        var age = ages.Where(i => i.weight >= minW & maxW >= i.weight).OrderBy(i => Random.value).ElementAt(0);

        weight += age.weight;
        return age.GetValue();
    }

    int GenerateRandomWeight(float height)
    {
        int minW = minGW - weight;//-5 - 0
        int maxW = maxGW - weight;//0 - 5
        var _weight = weights.Where(i => i.weight >= minW & maxW >= i.weight).OrderBy(i => Random.value).ElementAt(0);

        weight += _weight.weight;
        return _weight.GetValue(height);
    }

    void GenerateRandomFeatures()
    {
        var stats = person.GetModificator<BasicStats>();
        List<FeatureItem> features;

        if (stats.sex == PersonSex.Man)
        {
            features = womansCommonFeatures.OrderBy(i => Random.value).Take(2).ToList();
        }
        else
        {
            features = womansCommonFeatures.OrderBy(i => Random.value).Take(2).ToList();
        }

        foreach (var _feature in features)
        {
            weight += _feature.weight;
            person.AddModificator((PersonModificator)Activator.CreateInstance(_feature.type));
        }

    }

    void GenerateRandomNationalFeature()
    {
        var feature = nationalFeatures[Random.Range(0, nationalFeatures.Count)];
        weight += feature.weight + 1;
        person.AddModificator((PersonModificator)Activator.CreateInstance(feature.type));
    }
    void Start()
    {
        //men common features
        mansCommonFeatures.Add(new FeatureItem(typeof(AlcoholicMod), false));
        mansCommonFeatures.Add(new FeatureItem(typeof(WhinerMod), true));
        mansCommonFeatures.Add(new FeatureItem(typeof(JoillerMod), false));
        mansCommonFeatures.Add(new FeatureItem(typeof(BodybuilderMod), true));
        mansCommonFeatures.Add(new FeatureItem(typeof(SchizophrenicMod), false));
        mansCommonFeatures.Add(new FeatureItem(typeof(WarhammerMod), false));//difference
        mansCommonFeatures.Add(new FeatureItem(typeof(MisogynistMod), false));//difference
        mansCommonFeatures.Add(new FeatureItem(typeof(IncelMod), false)); //difference
        mansCommonFeatures.Add(new FeatureItem(typeof(WomanizerMod), false));

        //women commnon features
        womansCommonFeatures.Add(new FeatureItem(typeof(AlcoholicMod), false));
        womansCommonFeatures.Add(new FeatureItem(typeof(WhinerMod), true));
        womansCommonFeatures.Add(new FeatureItem(typeof(JoillerMod), false));
        womansCommonFeatures.Add(new FeatureItem(typeof(BodybuilderMod), true));
        womansCommonFeatures.Add(new FeatureItem(typeof(SchizophrenicMod), false));
        womansCommonFeatures.Add(new FeatureItem(typeof(FeministMod), false)); //difference
        womansCommonFeatures.Add(new FeatureItem(typeof(WomanizerMod), false));

        //national
        nationalFeatures.Add(new FeatureItem(typeof(JewishMod), false));
        nationalFeatures.Add(new FeatureItem(typeof(ArabMod), false));
        nationalFeatures.Add(new FeatureItem(typeof(BlackMod), true));
        nationalFeatures.Add(new FeatureItem(typeof(AsianMod), false));

        ages.Add(new AgeItem(20, 40, 0));
        ages.Add(new AgeItem(20, 40, 0));
        ages.Add(new AgeItem(20, 40, 0));
        ages.Add(new AgeItem(40, 60, 1));
        ages.Add(new AgeItem(40, 60, 1));
        ages.Add(new AgeItem(60, 80, 2));
        ages.Add(new AgeItem(80, 101, 3));

        weights.Add(new WeightItem(10, 15, 2));
        weights.Add(new WeightItem(15, 20, 1));
        weights.Add(new WeightItem(20, 25, 0));
        weights.Add(new WeightItem(20, 25, 0));
        weights.Add(new WeightItem(20, 25, 0));
        weights.Add(new WeightItem(15, 20, 1));
        weights.Add(new WeightItem(25, 35, 1));
        weights.Add(new WeightItem(35, 50, 2));
        weights.Add(new WeightItem(50, 70, 3));

        educations.Add(new EducationItem(PersonEducation.InternetCourses, 3));            //курсы 3
        educations.Add(new EducationItem(PersonEducation.IncompliteSecondarySchool, 2));  //неоконченная школа 2
        educations.Add(new EducationItem(PersonEducation.SecondarySchool, 1));            //средняя школа 1
        educations.Add(new EducationItem(PersonEducation.Bachelor, 0));                   //бакалавр 0
        educations.Add(new EducationItem(PersonEducation.Master, -1));                    //магистр 1
        educations.Add(new EducationItem(PersonEducation.ScienceDoctor, -2));             //доктор наук 2
        CreateNewPerson();
    }



    static List<FeatureItem> mansCommonFeatures = new List<FeatureItem>();
    static List<FeatureItem> womansCommonFeatures = new List<FeatureItem>();
    static List<FeatureItem> nationalFeatures = new List<FeatureItem>();

    static List<AgeItem> ages = new List<AgeItem>();
    static List<WeightItem> weights = new List<WeightItem>();
    static List<EducationItem> educations = new List<EducationItem>();
    class FeatureItem
    {
        public Type type;
        public int weight;
        public FeatureItem(Type type, bool isHighWeight)
        {
            this.type = type;
            if (isHighWeight)
            {
                weight = 1;
            }
            else
            {
                weight = 0;
            }
        }
    }
    class AgeItem
    {
        int min;
        int max;
        public int weight { get; }
        public AgeItem(int min, int max, int weight)
        {
            this.min = min;
            this.max = max;
            this.weight = weight;
        }

        public int GetValue()
        {
            return Random.Range(min, max);
        }
    }

    class WeightItem
    {
        int min;
        int max;
        public int weight { get; }
        public WeightItem(int min, int max, int weight)
        {
            this.min = min;
            this.max = max;
            this.weight = weight;
        }

        public int GetValue(float height)
        {
            return Mathf.RoundToInt(Random.Range(min, max) * height * height);
        }
    }

    class EducationItem
    {
        public PersonEducation type { get; }
        public int weight { get; }
        public EducationItem(PersonEducation type, int weight)
        {
            this.type = type;
            this.weight = weight;
        }
    }
}

