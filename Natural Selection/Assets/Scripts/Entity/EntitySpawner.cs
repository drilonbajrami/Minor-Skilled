using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class EntitySpawner : MonoBehaviour
{
    [Range(0.01f, 1f)]
    public float mutationFactor;
    [Range(0.0f, 100.0f)]
    public float mutationChance;

    // Start Positions and two lists for keeping track of available and unavailable start positions
    private Vector3[] startPositions;
    private List<int> availablePositions;
    private List<int> unavailablePositions;

    // Cache the bounds of the ground plane
    private Bounds bounds;

    // Keep track of current active entities
    private List<GameObject> entities;

    private ObjectPooler entityPooler;

    // List of species to be created
    public List<Specie> species;

    public static int ActiveEntities = 0;
    public static bool EntitiesNotReachedDestinationsYet = true;
    public static bool HasCycleEnded = false;
    public static bool ReadyToStartNewCycle = true;

    public Text herbivoreCount;
    public Text carnivoreCount;

    public static int hCount;
    public static int cCount;

    void Start()
    {
        bounds = GameObject.FindGameObjectWithTag("Ground").gameObject.GetComponent<MeshCollider>().bounds;
        entityPooler = gameObject.GetComponent<ObjectPooler>();
        entities = new List<GameObject>();
        SetupStartPositions();
        Cycle.CycleStart += OnCycleStart;
        Cycle.CycleEnd += OnCycleEnd;
    }

	private void Update()
	{
        if (HasCycleEnded && ActiveEntities == entities.Count && entities.Count != 0 && EntitiesNotReachedDestinationsYet)
        {
            EntitiesNotReachedDestinationsYet = false;
            ReadyToStartNewCycle = true;
        }

        herbivoreCount.text = $"Herbivore count: {hCount}";
        carnivoreCount.text = $"Carnivore count: {cCount}";

    }

	private void OnCycleStart(object sender, EventArgs eventArgs)
    {
        ActiveEntities = 0;
        ReadyToStartNewCycle = false;
        HasCycleEnded = false;
        EntitiesNotReachedDestinationsYet = true;

        if (Cycle.cycleCount == 0)
        {
            SpawnFirstGenerationOfEntities();
            GetActiveEntities();
            FirstCyclePositioning();
        }
        else
        {
            GetActiveEntities();
            CrossbreedTheFittestEntities();
            ResetStartingPositions();
        }
    }

    private void OnCycleEnd(object sender, EventArgs eventArgs)
    {
        GetActiveEntities();
        SendAllEntitiesBackToStartingPositions();
        HasCycleEnded = true;
    }

    private void SpawnFirstGenerationOfEntities()
    {
        for (int s = 0; s < species.Count; s++)
        {
            for (int i = 0; i < species[s].numberOfEntities; i++)
            {
                GameObject o = entityPooler.SpawnFromPool("Entity", GetRandomStartingPosition(), Quaternion.identity);
                Entity e = o.GetComponent<Entity>();
                e.transform.LookAt(Vector3.zero);

                ColorAllele colorA = species[s].colorAlleles[Random.Range(0, species[s].numberOfColorAlleles)].colorAllele;
                ColorAllele colorB = species[s].colorAlleles[Random.Range(0, species[s].numberOfColorAlleles)].colorAllele;

                SizeAllele sizeA = species[s].sizeAlleles[Random.Range(0, species[s].numberOfSizeAlleles)].sizeAllele;
                SizeAllele sizeB = species[s].sizeAlleles[Random.Range(0, species[s].numberOfSizeAlleles)].sizeAllele;

                SexAllele sexA;
                SexAllele sexB;

                BehaviorAllele behaviorA = species[s].behaviorAlleles[Random.Range(0, species[s].numberOfBehaviorAlleles)].behaviorAllele;

                if (i < species[s].numberOfFemales)
                {
                    sexA = species[s].sexAlleles[0].sexAllele;
                    sexB = species[s].sexAlleles[0].sexAllele;
                }
                else
                {
                    sexA = species[s].sexAlleles[0].sexAllele;
                    sexB = species[s].sexAlleles[1].sexAllele;
                }

                ColorGene color = new ColorGene(colorA.GetCopy(0.25f, 70.0f), colorB.GetCopy(0.25f, 70.0f));
                SizeGene size = new SizeGene(sizeA.GetCopy(0.10f, 50.0f), sizeB.GetCopy(0.10f, 50.0f));
                SexGene sex = new SexGene(sexA.GetCopy(0, 0), sexB.GetCopy(0, 0));
                BehaviorGene behavior = new BehaviorGene(behaviorA.GetCopy(0, 0), behaviorA.GetCopy(0, 0));

                e.genome = new Genome(color, size, sex, behavior);
                e.ExpressGenome();

                if (s == 0)
                {
                    e.order = Order.HERBIVORE;
                    hCount++;
                }
                else
                {
                    e.order = Order.CARNIVORE;
                    cCount++;
                }
            }
        }
    }

    private void FirstCyclePositioning()
    {
        foreach (GameObject entity in entities) entity.transform.position = GetRandomStartingPosition();
        ResetStartingPositions();
    }

    private void CrossbreedTheFittestEntities()
    {
        List<GameObject> herbivoreFemales = new List<GameObject>();
        List<GameObject> herbivoreMales = new List<GameObject>();

        List<GameObject> carnivoreFemales = new List<GameObject>();
        List<GameObject> carnivoreMales = new List<GameObject>();

        foreach (GameObject entity in entities)
        {
            Entity e = entity.GetComponent<Entity>();
            if (e.order == Order.HERBIVORE)
            {
                if (e.gender == Gender.FEMALE)
                    herbivoreFemales.Add(entity);
                else
                    herbivoreMales.Add(entity);
            }
            else
            {
                if (e.gender == Gender.FEMALE)
                    carnivoreFemales.Add(entity);
                else
                    carnivoreMales.Add(entity);

            }
        }

        herbivoreFemales.Sort(delegate (GameObject a, GameObject b)
        { return (b.GetComponent<Entity>().Fitness).CompareTo(a.GetComponent<Entity>().Fitness); }
                             );

        herbivoreMales.Sort(delegate (GameObject a, GameObject b)
        { return (b.GetComponent<Entity>().Fitness).CompareTo(a.GetComponent<Entity>().Fitness); }
                           );

        carnivoreFemales.Sort(delegate (GameObject a, GameObject b)
        { return (b.GetComponent<Entity>().Fitness).CompareTo(a.GetComponent<Entity>().Fitness); }
                           );

        carnivoreMales.Sort(delegate (GameObject a, GameObject b)
        { return (b.GetComponent<Entity>().Fitness).CompareTo(a.GetComponent<Entity>().Fitness); }
                           );

        if (herbivoreFemales.Count != 0 && herbivoreMales.Count != 0)
        {
            int maxFemaleCount = herbivoreFemales.Count >= 20 ? 20 : herbivoreFemales.Count;
            int maxMaleCount = herbivoreMales.Count >= 10 ? 10 : herbivoreMales.Count;

            for (int i = 0; i < maxFemaleCount; i++)
            {
                GameObject o = entityPooler.SpawnFromPool("Entity", GetRandomStartingPosition(), Quaternion.identity);
                Entity e = o.GetComponent<Entity>();
                e.transform.LookAt(Vector3.zero);

                e.genome = herbivoreFemales[i].GetComponent<Entity>().genome.CrossGenome(herbivoreMales[Random.Range(0, maxMaleCount)].GetComponent<Entity>().genome, mutationFactor, mutationChance);
                e.order = Order.HERBIVORE;
                e.ExpressGenome();
                entities.Add(o);
                hCount++;
            }
        }

        if (carnivoreFemales.Count != 0 && carnivoreMales.Count != 0)
        {
            int maxFemaleCount = carnivoreFemales.Count >= 3 ? 3 : carnivoreFemales.Count;
            int maxMaleCount = carnivoreMales.Count >= 3 ? 3 : carnivoreMales.Count;

            for (int i = 0; i < maxFemaleCount; i++)
            {
                GameObject o = entityPooler.SpawnFromPool("Entity", GetRandomStartingPosition(), Quaternion.identity);
                Entity e = o.GetComponent<Entity>();
                e.transform.LookAt(Vector3.zero);
                e.genome = carnivoreFemales[i].GetComponent<Entity>().genome.CrossGenome(carnivoreMales[Random.Range(0, maxMaleCount)].GetComponent<Entity>().genome, mutationFactor, mutationChance);
                e.order = Order.CARNIVORE;
                e.ExpressGenome();
                entities.Add(o);
                cCount++;
            }
        }
    }

    /// Get all current active entities
    private void GetActiveEntities()
    {
        entities.Clear();

        for (int i = 0; i < gameObject.transform.childCount; i++)
            if (gameObject.transform.GetChild(i).gameObject.activeSelf)
                entities.Add(gameObject.transform.GetChild(i).gameObject);
    }

    /// Generate the random starting positions
    private void SetupStartPositions()
    {
        availablePositions = new List<int>();
        unavailablePositions = new List<int>();

        startPositions = new Vector3[400];          // There will not be more than 400 entities at once in the simulation
        int halfLength = (int)bounds.max.x - 1;     // Half length of the ground excluding the edges
        int distanceInterval = 3;                   // distance inbetween start positions
        int positionIndex = 0;                      // indexer for startPositions array

        // Top and Bottom spawning positions
        for (int z = -halfLength; z < halfLength + 1; z += 2 * halfLength)
        {
            for (int x = -halfLength; x < halfLength; x += distanceInterval)
            {
                if (positionIndex >= startPositions.Length)
                    break;

                startPositions[positionIndex] = new Vector3(x, 0, z);
                availablePositions.Add(positionIndex);
                positionIndex++;
            }
        }

        // Left and Right starting positions
        for (int x = -halfLength; x < halfLength + 1; x += 2 * halfLength)
        {
            for (int z = -halfLength; z < halfLength; z += distanceInterval)
            {
                if (positionIndex >= startPositions.Length)
                    break;

                startPositions[positionIndex] = new Vector3(x, 0, z);
                availablePositions.Add(positionIndex);
                positionIndex++;
            }
        }
    }

    /// Use the given starting position and mark it as unavailable
    private void UseStartingPosition(int index)
    {
        // Store the position index as used
        unavailablePositions.Add(availablePositions[index]);

        // Remove position index from available positions
        availablePositions.RemoveAt(index);
    }

    /// Get a random starting position from the array and use it
    private Vector3 GetRandomStartingPosition()
    {
        int index = Random.Range(0, availablePositions.Count);
        Vector3 pos = startPositions[availablePositions[index]];
        UseStartingPosition(index);
        return pos;
    }

    /// Reset all unavailable starting positions to available
    private void ResetStartingPositions()
    {
        for (int i = 0; i < unavailablePositions.Count; i++)
            availablePositions.Add(unavailablePositions[i]);

        unavailablePositions.Clear();
    }

    private void SendAllEntitiesBackToStartingPositions()
    {
        foreach (GameObject entity in entities)
            entity.GetComponent<Entity>().SetDestination(GetRandomStartingPosition());
    }

    //private Vector3 GetRandomPosition(float margin)
    //{
    //    Vector3 position = new Vector3();
    //    position.x = Random.Range(bounds.min.x + margin, bounds.max.x - margin);
    //    position.z = Random.Range(bounds.min.z + margin, bounds.max.z - margin);
    //    position.y = 0.5f;

    //    if (usedPositions.Count != 0)
    //        foreach (Vector3 usedPos in usedPositions)
    //            if (Vector3.Distance(usedPos, position) < 2.0f)
    //                position = GetRandomPosition(margin);

    //    return position;
    //}
}