using System;
using System.Linq;


namespace Project_2_Genetic_Algorithm
{
    class Program
    {
        public static List<int> ProbWheel(int[] probability, int popsize)
        {
            List<int> indexs = new List<int>();
            for(int i = 0; i < popsize; i++)
            {
                for(int j = 0; j < probability[i]; j++)
                {
                    indexs.Add(i);
                }
            }
            return indexs;
        }

        public static bool[,] Selection(List<int> probwheel, bool[,] population, int numitems)
        {
            bool[,] parents = new bool[2, numitems];
            int parent1;
            int parent2;
            var random  = new Random();

            parent1 = random.Next(probwheel.Count);
            do
            {
                parent2 = random.Next(probwheel.Count);
            } while (probwheel[parent1] == probwheel[parent2]);


           Console.Write("\nSelected Parents {0} and {1}, ", probwheel[parent1] + 1, probwheel[parent2] + 1);
            


            for (int j = 0; j < numitems; j++)
            {
               parents[0, j] = population[probwheel[parent1], j];
               parents[1, j] = population[probwheel[parent2], j];
            }
            return parents;
        }

        public static bool[,] GenAlg(int maxgen, double[] weights, double[] values, int noOfitems, int populationSize, double WeightCapacity)
        {
            List<double> bestFit = new List<double>();
            List<double> bestGenFit = new List<double>();

            Random rand = new Random();
            bool[,] population = new bool[populationSize, noOfitems];

            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < noOfitems; j++)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        population[i, j] = true;
                    }
                    else
                    {
                        population[i, j] = false;
                    }
                }
            }

            //Display the Initial Population


            for (int generations = 0; generations < maxgen; generations++)
            {
                Console.Write("\n\n---[ GENERATION {0} ]---", generations + 1);
                //Step 2: Evaluate Fitness Function
                double[] fitnessVal = new double[populationSize];  //fitness value of knapsack
                double[] sumWeights = new double[populationSize];  //Sum of Weights

                for (int i = 0; i < populationSize; i++)
                {
                    fitnessVal[i] = 0;
                    sumWeights[i] = 0;
                }

                for (int i = 0; i < populationSize; i++)
                {
                    for (int j = 0; j < noOfitems; j++)
                    {
                        if (population[i, j])
                        {
                            fitnessVal[i] = fitnessVal[i] + values[j];
                            sumWeights[i] = sumWeights[i] + weights[j];
                        }
                    }

                    Console.Write("\nFitness value and weight of Chromosome {0} : {1} and {2}", i + 1, fitnessVal[i], sumWeights[i]);
                }

                //Display the acceptable and discardable Chromosomes by comparing their weights with capacity  weight 

                // double[] accepted = new double[populationSize];
                // double[] discarded = new double[populationSize];
                Console.Write("\n");
                for (int i = 0; i < populationSize; i++)
                {
                    if (sumWeights[i] >= WeightCapacity || sumWeights[i] == 0)
                    {
                        fitnessVal[i] = 0;
                        Console.Write("\nChromosome {0} is discarded", i + 1);
                    }
                    else
                    {
                        //accepted[i] = fitnessVal[i];
                        Console.Write("\nChromosome {0} is accepted", i + 1);
                    }
                }

                //Step 3: Selection

                //Find the best/largest fitness value
                double bestFitnessVal;
                int bestFitnessIndex;
                bestFitnessVal = fitnessVal.Max();
                bestFitnessIndex = fitnessVal.ToList().IndexOf(bestFitnessVal);

                Console.Write("\n\nLargest Fitness Value is of Chromosome {0}: {1}", bestFitnessIndex + 1, bestFitnessVal);

                //Find Total Fitness Value
                double totalFitness = 0;
                for (int i = 0; i < populationSize; i++)
                {
                    totalFitness += fitnessVal[i];
                }

                Console.Write("\n\nTotal Fitness Value: {0}", totalFitness);





                //Convergence Function
                double fitDif = 0;
                //int converg


                bestGenFit.Add(totalFitness);
                bestFit.Add(bestFitnessVal);



                if (generations >= (7) + 1)
                {
                    for(int i = generations; i >= generations - (7); i--)
                    {
                        fitDif = fitDif + Math.Abs((bestFit[i] - bestFit[i - 1]));
                    }

                    
                    if (fitDif == 0)
                    {
                        Console.Write("\nConvergence was detected at generation {0}.", generations);
                        for (int i = 0; i < generations; i++)
                        {
                            Console.Write("\nFitness for Generation {0}: {1}", i, bestFit[i]);
                            Console.Write("\nTotal fitness for Generation {0}: {1}", i, bestGenFit[i]);
                        }
                            
                            
                        return population;

                    }

                }
                



                


                //Calculate Probability
                Console.Write("\n\nCalculate Probability of each Chromosome:");
                double[] probability = new double[populationSize];
                int[] probabilityPercentage = new int[populationSize];

                for (int i = 0; i < populationSize; i++)
                {
                    probability[i] = fitnessVal[i] / totalFitness;
                    probabilityPercentage[i] = Convert.ToInt32(Math.Round(100 * probability[i]));

                    Console.Write("\nChromosome {0} has {1} % chance of being used.", i + 1, probabilityPercentage[i]);
                }

                Console.Write("\n\nChromosome {0} has the highest probability of being chosen.\n", bestFitnessIndex + 1);
                List<int> randWheel = new List<int>();
                randWheel = ProbWheel(probabilityPercentage, populationSize);

                bool[,] parents = new bool[2, noOfitems];
                bool[,] childs = new bool[2, noOfitems];
                bool[,] newPopulation = new bool[populationSize, noOfitems];



                for (int i = 0; i < populationSize; i = i + 2)
                {
                    parents = Selection(randWheel, population, noOfitems);

                    childs = Crossover(parents, noOfitems);
                    Console.Write("for children {0} and {1}.", i + 1, i + 2);
                    for (int j = 0; j < noOfitems; j++)
                    {
                        newPopulation[i, j] = childs[0, j];
                        newPopulation[i + 1, j] = childs[1, j];         //EVEN POPULATIONS ONLY

                    }
                }

                population = newPopulation;
                Console.Write("\n");

            }
            for (int i = 0; i < maxgen; i++)
            {
                Console.Write("\nFitness for Generation {0}: {1}", i, bestFit[i]);
                Console.Write("\nTotal fitness for Generation {0}: {1}", i, bestGenFit[i]);
            }
            return population;
        }
        public static bool[,] Crossover(bool[,] parents, int numitems)
        {
            var random = new Random();
            bool[,] child = new bool [2, numitems];
            int crossover1 = random.Next(parents.GetLength(0)); //Get Random Index from 1st Parent Chromosome 
            
            for(int i = 0; i < numitems; i++)
            {
                if (i <= crossover1)
                {
                    child[0, i] = parents[0, i];
                    child[1, i] = parents[1, i];
                }
                else
                {
                    child[0, i] = parents[1, i];
                    child[1, i] = parents[0, i];
                }
            }

            //bool mutate;
            //Find chromosome and its one index value randomly
            //int mutateIndexRow = random.Next(0, populationSize - 1);
            //int mutateIndexCol = random.Next(0, noOfitems - 1);
            decimal mutchance = 0.05M; //make this editable
            var rand = new Random();
            var mutroll1 = new decimal(rand.NextDouble());
            var mutroll2 = new decimal(rand.NextDouble());
            int randbit;

            for(int i = 0; i < numitems; i++)
            {
                mutroll1 = new decimal(rand.NextDouble());
                mutroll2 = new decimal(rand.NextDouble());
                if (mutroll1 < mutchance)
                {
                    
                    if (child[0, i] == false)
                    {
                        child[0, i] = true;
                    }
                    else
                    {
                        child[0, i] = false;
                    }
                    


                }

                if (mutroll2 < mutchance)
                {
                    
                    if (child[1, i] == false)
                    {
                        child[1, i] = true;
                    }
                    else
                    {
                        child[1, i] = false;
                    }
                    
                }
            }
            

            return child;
           
        }
        static void Main(string[] args)
        {


            /*
            Console.Write("Use default set?");
            string ans;
            ans = Convert.ToString(Console.ReadLine());
            */
            int maxgen = 50;
            double[] weights = { 4, 2, 1, 1, 3, 2, 1, 2, 3, 4 };
            double[] values = { 140, 40, 80, 20, 100, 130, 20, 30, 50, 80 };

            int noOfitems = 10;
            int populationSize = 14; //MUST BE EVEN
            double WeightCapacity = 12;

            string[] items = { "Laptop", "Notebook", "Pencil", "Calculator", "Lab Kit", "Phone", "Thing 1", "Thing 2", "Thing 3", "Thing 4" };

            bool[,] popFunction = new bool[noOfitems, populationSize];
            popFunction = GenAlg(maxgen, weights, values, noOfitems, populationSize, WeightCapacity);
            Console.Write("\n\n");








            /*
                Console.Write("Enter Maximum Weight Capacity of Knapsack in Kg : ");
                double WeightCapacity = Convert.ToDouble(Console.ReadLine()); //Maximum capacity of Knapsack

                Console.Write("\n\nEnter No. of Items : ");

                noOfitems = Int32.Parse(Console.ReadLine());
                string[] items = new string[noOfitems];
                double[] weights = new double[noOfitems];
                double[] values = new double[noOfitems];


                //Take input from user to enter items
                Console.WriteLine();

                for (int i = 0; i < noOfitems; i++)
                {
                    Console.Write("Enter Name of item {0} : ", i + 1);
                    items[i] = Convert.ToString(Console.ReadLine());
                }

                Console.WriteLine();

                //Take input from user to enter weights of items

                for (int i = 0; i < noOfitems; i++)
                {
                    Console.Write("Enter Weight of item {0} : ", items[i]);
                    weights[i] = Convert.ToDouble(Console.ReadLine());
                }

                Console.WriteLine();

                //Take input from user to enter values of items

                for (int i = 0; i < noOfitems; i++)
                {
                    Console.Write("Enter Value of item {0} : ", items[i]);
                    values[i] = Convert.ToDouble(Console.ReadLine());
                }

                Console.Clear();

                //Display the user inputs in the form of table

                Console.WriteLine("\nITEMS\t\tWEIGHTS\t\tVALUES");
                Console.WriteLine("________________________________________________________\n");
                for (int i = 0; i < noOfitems; i++)
                {
                    Console.Write("{0}\t\t{1}\t\t{2}\n", items[i], weights[i], values[i]);
                }
            
            //Now solve this KnapSack problem using Genetic Algorithm


            //Step 1: Initialize random initial population 
            //Chromosome Encoding

            
                int populationSize = new int();

                Console.Write("\nEnter Population Size: ");
                populationSize = Convert.ToInt32(Console.ReadLine());
            */

            /*
            Random rand = new Random();
            bool[,] population = new bool[populationSize, noOfitems];

            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < noOfitems; j++)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        population[i, j] = true;
                    }
                    else
                    {
                        population[i, j] = false;
                    }
                }
            }

            //Display the Initial Population
            

            for(int generations = 0;  generations < maxgen; generations++)
            {
                    Console.Write("\n\n---[ GENERATION {0} ]---", generations + 1);
                    //Step 2: Evaluate Fitness Function
                    double[] fitnessVal = new double[populationSize];  //fitness value of knapsack
                    double[] sumWeights = new double[populationSize];  //Sum of Weights

                    for (int i = 0; i < populationSize; i++)
                    {
                        fitnessVal[i] = 0;
                        sumWeights[i] = 0;
                    }

                    for (int i = 0; i < populationSize; i++)
                    {
                        for (int j = 0; j < noOfitems; j++)
                        {
                            if (population[i, j])
                            {
                                fitnessVal[i] = fitnessVal[i] + values[j];
                                sumWeights[i] = sumWeights[i] + weights[j];
                            }
                        }

                        Console.Write("\nFitness value and weight of Chromosome {0} : {1} and {2}", i + 1, fitnessVal[i], sumWeights[i]);
                    }

                    //Display the acceptable and discardable Chromosomes by comparing their weights with capacity  weight 

                    // double[] accepted = new double[populationSize];
                    // double[] discarded = new double[populationSize];
                    Console.Write("\n");
                    for (int i = 0; i < populationSize; i++)
                    {
                        if (sumWeights[i] >= WeightCapacity || sumWeights[i] == 0)
                        {
                            fitnessVal[i] = 0;
                            Console.Write("\nChromosome {0} is discarded", i + 1);
                        }
                        else
                        {
                            //accepted[i] = fitnessVal[i];
                            Console.Write("\nChromosome {0} is accepted", i + 1);
                        }
                    }

                    //Step 3: Selection

                    //Find the best/largest fitness value
                    double bestFitnessVal;
                    int bestFitnessIndex;
                    bestFitnessVal = fitnessVal.Max();
                    bestFitnessIndex = fitnessVal.ToList().IndexOf(bestFitnessVal);

                    Console.Write("\n\nLargest Fitness Value is of Chromosome {0}: {1}", bestFitnessIndex + 1, bestFitnessVal);

                    //Find Total Fitness Value
                    double totalFitness = 0;
                    for (int i = 0; i < populationSize; i++)
                    {
                        totalFitness += fitnessVal[i];
                    }

                    Console.Write("\n\nTotal Fitness Value: {0}", totalFitness);


                    //Calculate Probability
                    Console.Write("\n\nCalculate Probability of each Chromosome:");
                    double[] probability = new double[populationSize];
                    int[] probabilityPercentage = new int[populationSize];

                    for (int i = 0; i < populationSize; i++)
                    {
                        probability[i] = fitnessVal[i] / totalFitness ;
                        probabilityPercentage[i] = Convert.ToInt32(Math.Round(100 * probability[i]));

                        Console.Write("\nChromosome {0} has {1} % chance of being used.", i + 1, probabilityPercentage[i]);
                    }

                    Console.Write("\n\nChromosome {0} has the highest probability of being chosen.\n", bestFitnessIndex + 1);
                    List<int> randWheel = new List<int>();
                    randWheel = ProbWheel(probabilityPercentage, populationSize);

                    bool[,] parents = new bool[2, noOfitems];
                    bool[,] childs = new bool[2, noOfitems];
                    bool[,] newPopulation = new bool[populationSize, noOfitems];



                    for (int i = 0; i < populationSize; i = i + 2)
                    {
                        parents = Selection(randWheel, population, noOfitems);
                        
                        childs = Crossover(parents, noOfitems);
                        Console.Write("for children {0} and {1}.", i + 1, i + 2);
                        for (int j = 0; j < noOfitems; j++)
                        {
                            newPopulation[i, j] = childs[0, j];
                            newPopulation[i + 1, j] = childs[1, j];         //EVEN POPULATIONS ONLY

                        }
                    }

                    population = newPopulation;
                Console.Write("\n");

                }







            /*
                Console.WriteLine();
            Console.WriteLine("\t--------------Generation : 1-----------------\n\n");
            for (int i = 0; i < populationSize; i++)
            {
                Console.Write("Chromosome {0} : ", i + 1);

                for (int j = 0; j < noOfitems; j++)
                {
                    Console.Write("\t {0}", Convert.ToInt32(population[i, j]));
                }

                Console.WriteLine();
            }

            

            double secondBestFitness;
            secondBestFitness = fitnessVal.OrderByDescending(z => z).Skip(1).First();
            int secondBestFitnessIndex;


            secondBestFitnessIndex = fitnessVal.ToList().IndexOf(secondBestFitness);



            Console.Write("\nSelected Chromosomes / Parents\n");


            for (int j = 0; j < noOfitems; j++)
            {
                parents[0, j] = population[bestFitnessIndex, j];
                parents[1, j] = population[secondBestFitnessIndex, j];
            }


            Console.WriteLine("\n\n\t--------------Generation : 2-----------------\n\n");

            Console.Write("\nChromosome {0} : ", bestFitnessIndex + 1);
            for (int i = 0; i < noOfitems; i++)
            {
                Console.Write("\t" + Convert.ToInt32(parents[0, i]));
            }

            Console.Write("\nChromosome {0} : ", secondBestFitnessIndex + 1);
            for (int i = 0; i < noOfitems; i++)
            {
                Console.Write("\t" + Convert.ToInt32(parents[1, i]));
            }
            /*
            //Step 4:  Crossover point

            int crosspointindex1 = rand.Next(parents.GetLength(0)); //Get Random Index from 1st Parent Chromosome 
            int crosspointindex2 = rand.Next(parents.GetLength(1)); //Get Random Index from 2nd Parent Chromosome 

            bool crosspoint1 = parents[0, crosspointindex1];
            bool crosspoint2 = parents[1, crosspointindex2];

            //int crossoverindex = crosspointindex1;
            //crosspointindex1 = crosspointindex2;
            //crosspointindex2 = crossoverindex;

            bool crossover = crosspoint1;
            crosspoint1 = crosspoint2;
            crosspoint2 = crossover;
            */

            /*
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < noOfitems; j++)
                {
                    childs[i, j] = parents[i, j];
                }
            }

            childs[0, crosspointindex1] = crosspoint1;
            childs[1, crosspointindex2] = crosspoint2;

            //Display Generation 3

            Console.WriteLine("\n\n\t--------------Generation : 3-----------------\n\n");

            for (int i = 0; i < 2; i++)
            {
                Console.Write("Parent {0} : ", i + 1);

                for (int j = 0; j < noOfitems; j++)
                {
                    Console.Write("\t" + Convert.ToInt32(parents[i, j]));
                }

                Console.WriteLine();
            }

            for (int i = 0; i < 2; i++)
            {
                Console.Write("Child {0} : ", i + 1);

                for (int j = 0; j < noOfitems; j++)
                {
                    Console.Write("\t" + Convert.ToInt32(childs[i, j]));
                }

                Console.WriteLine();
            }


            //Store parents and childs/offsprings to new population

            
            int k;
            for (k = 0; k < 2; k++)
            {
                for (int j = 0; j < noOfitems; j++)
                {
                    newPopulation[k, j] = parents[k, j];
                }
            }

            int c = 0;
            for (k = populationSize - 2; k < populationSize; k++)
            {
                for (int j = 0; j < noOfitems; j++)
                {
                    newPopulation[k, j] = childs[c, j];
                }
                c++;
            }

            //Step 5: Mutation

            bool mutate;
            //Find chromosome and its one index value randomly
            int mutateIndexRow = rand.Next(0, populationSize - 1);
            int mutateIndexCol = rand.Next(0, noOfitems - 1);

            mutate = newPopulation[mutateIndexRow, mutateIndexCol];

            //Flip the  bit

            if (!mutate)
            {
                mutate = true;
            }
            else if (mutate)
            {
                mutate = false;
            }

            newPopulation[mutateIndexRow, mutateIndexCol] = mutate;
            Console.Write("\n\n----------After Mutation----------\n\n");
            Console.Write("Chromosome {0} : ", mutateIndexRow + 1);

            for (int i = 0; i < noOfitems; i++)
            {
                Console.Write("\t" + Convert.ToInt32(newPopulation[mutateIndexRow, i]));
            }

            //  newPopulation[mutateIndex] 
            */
        }
    }
}
