using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
//using Random = UnityEngine.Random;

class PersonGenerator : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;
    const int requirementGW = 4;
    const int maxGW = requirementGW + 2;
    const int minGW = requirementGW - 3;

    static PersonIdentety person;
    int weight;
    public static PersonGenerator Instance;
    System.Random random;

    public PersonIdentety CreateNewPerson(int seed = 0)
    {
        if(seed == 0)
        {
            seed = UnityEngine.Random.Range(-999999, 999999);
        }
        random = new System.Random(seed);
        GameObject _person = Instantiate(personPrefab);

        _person.transform.position = transform.position;

        person = _person.GetComponent<PersonIdentety>();
        weight = 0;

        PersonSex sex = PersonSex.Man;
        if (random.Next(0, 2) == 1)
        {
            sex = PersonSex.Woman;
        }
        var stats = new BasicStats();

        stats.status = PersonStatus.hidden;
        stats.sex = sex;
        stats.seed = seed;

        person.AddModificator(stats);

        GenerateRandomFeatures();


        if (random.Next(0, 4) == 3)
        {
            GenerateRandomNationalFeature();
        }

        float height;
        int age;
        int personWeight;

        if (sex == PersonSex.Man)
        {
            height = random.Next(160, 210);
        }
        else
        {
            height = random.Next(140, 190);
        }

        height = height / 100;

        if (random.Next(0, 2) == 1)
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

        int orientationChance = random.Next(1, 101);

        if(orientationChance <= 50)
        {
            stats.orientation = PersonOrientation.Hetero;
        }
        else if (orientationChance <= 70)
        {
            stats.orientation = PersonOrientation.Gay;
        }
        else if (orientationChance <= 90)
        {
            stats.orientation = PersonOrientation.Bi;
        }
        else
        {
            stats.orientation = PersonOrientation.A;
        }

        stats.education = education;
        stats.age = age;
        stats.height = height;
        stats.weight = personWeight;

        return person;
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

        var age = ages.Where(i => i.weight >= minW & maxW >= i.weight).OrderBy(i => (float)random.NextDouble()).ElementAt(0);

        weight += age.weight;
        return age.GetValue(random);
    }

    int GenerateRandomWeight(float height)
    {
        int minW = minGW - weight;//-5 - 0
        int maxW = maxGW - weight;//0 - 5
        var _weight = weights.Where(i => (i.weight >= minW & maxW >= i.weight)).OrderBy(i => (float)random.NextDouble()).ElementAt(0);

        weight += _weight.weight;
        return _weight.GetValue(height,random);
    }

    void GenerateRandomFeatures()
    {
        var stats = person.GetModificator<BasicStats>();
        List<FeatureItem> features;

        if (stats.sex == PersonSex.Man)
        {
            features = womansCommonFeatures.OrderBy(i => (float)random.NextDouble()).Take(2).ToList();
        }
        else
        {
            features = womansCommonFeatures.OrderBy(i => (float)random.NextDouble()).Take(2).ToList();
        }

        foreach (var _feature in features)
        {
            weight += _feature.weight;
            person.AddModificator((PersonModificator)Activator.CreateInstance(_feature.type));
        }

    }

    void GenerateRandomNationalFeature()
    {
        var feature = nationalFeatures[random.Next(0, nationalFeatures.Count)];
        weight += feature.weight + 1;
        person.AddModificator((PersonModificator)Activator.CreateInstance(feature.type));
    }
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

        public int GetValue(System.Random random)
        {
            return random.Next(min, max);
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

        public int GetValue(float height, System.Random random)
        {
            return Mathf.RoundToInt(random.Next(min, max) * height * height);
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

