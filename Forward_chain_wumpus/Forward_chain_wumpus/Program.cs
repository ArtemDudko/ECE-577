using System;
using System.Linq;


namespace Forward_chain_wumpus
{
    class Program
    {


        static void Main(string[] args)
        {

            List<string> agenda = new List<string>();
            var KB = new List<Tuple<string,int,int>>();
            var Navigation = new List<Tuple<int, int>>();
            var Inferred = new List<Tuple<string, int, int>>();
            var UnexploredPossibleTiles = new List<Tuple<int, int>>();
            var Escape = new List<Tuple<int, int>>();
            int[] Trouble = { 0, 0 };
            var PossibleTiles = new List<Tuple<int, int>>();
            Random Chosen = new Random();
            bool move;
            int Index;
            int AdjacentTiles;
            int Safespace;
            int temp1;
            int temp2;
            //int[, , ,] Environment = { {} };
            //stench, wumpus, breeze, pit, gold
            bool[,,] Environment = { { {false, false, false, false, false}, { true , false, false, false, false }, { false, true , false, false, false }, { true , false, false, false, false } },
                                      { {false, false, true , false, false}, { false, false, false, false, false }, { true , false, true , false, true  }, { false, false, false, false, false } },
                                      { {false, false, false, true , false}, { false, false, true , false, false }, { false, false, false, true , false }, { false, false, true , false, false } },
                                      { {false, false, true , false, false}, { false, false, false, false, false }, { false, false, true , false, false }, { false, false, false, true , false } } };
            bool[,,] Known = new bool[4, 4, 5];
            //int[] PlayerPosition = new int[2];
            int x = 0; int y = 0; bool goldCarried = false;

            bool[,] Explored = new bool[4, 4]; //Environment dimensions

            string[] temp;


            while (true) 
            {

                //explored current tile

                if (Explored[x, y] == false) // new tile is explored
                {
                    Explored[x, y] = true; // explored

                    //check for gold and take it
                    if (Environment[x, y, 4] == true)
                    {
                        Console.Write("The Agent Found the Gold! Now Escape!\n\n"); // found gold!
                        Environment[x, y, 4] = false; // gold has been taken
                        goldCarried = true;
                    }

                    //Add current postion to KB variables
                    if (Environment[x, y, 1] == true) // wumpus tile
                    {
                        Console.WriteLine("Died to Wumpus"); // died to wumpus
                        break; // end program
                    }
                    else
                    {
                        if(KB.Contains(new Tuple<string, int, int>("~W", x, y)) == false) // check if knowledge base already contains not wumpus
                        {
                            KB.Add(new Tuple<string,int,int>( "~W", x, y)); // add not wumpus
                        }
                        
                    }

                    if (Environment[x, y, 3] == true) // tile contains pit
                    {
                        Console.WriteLine("Died to Pit"); // died to pit
                        break; // end program
                    }
                    else
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~P", x, y)) == false) // check if knowledge base already contains not pit
                        {
                            KB.Add(new Tuple<string, int, int>( "~P", x, y)); // add not pit
                        }
                        
                    }

                    if (Environment[x, y, 2] == true)       //add breeze to knowledge base
                    {
                        KB.Add(new Tuple<string, int, int>("B", x, y));
                    }
                    else
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~B", x, y)) == false) // add not breeze if not found
                        {
                            KB.Add(new Tuple<string, int, int>("~B", x, y));
                        }
                        
                        if (x + 1 != 4) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~P", x + 1, y)) == false) // add not pit if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~P", x + 1, y));
                            }
                            
                        }
                        if (x - 1 != -1) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~P", x - 1, y)) == false) // add not pit if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~P", x - 1, y));
                            }
                            
                        }
                        if (y + 1 != 4) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~P", x , y + 1)) == false) // add not pit if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~P", x, y + 1));
                            }
                            
                        }
                        if (y - 1 != -1) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~P", x, y - 1)) == false) // add not pit if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~P", x, y - 1));
                            }
                            
                        }
                    }

                    if (Environment[x, y, 0] == true)       //add stench to knowledgebase
                    {
                        KB.Add(new Tuple<string, int, int>("S", x, y));
                    }
                    else
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~S", x, y)) == false) // add not stench if not found
                        {
                            KB.Add(new Tuple<string, int, int>("~S", x, y));
                        }
                        
                        if (x + 1 != 4) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~W", x + 1, y)) == false) // add not wumpus if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~W", x + 1, y));
                            }
                            
                        }
                        if (x - 1 != -1) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~W", x - 1, y)) == false)  // add not wumpus if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~W", x - 1, y));
                            }
                            
                        }
                        if (y + 1 != 4) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~W", x, y + 1)) == false)  // add not wumpus if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~W", x, y + 1));
                            }
                            
                        }
                        if (y - 1 != -1) // bounds of environment
                        {
                            if (KB.Contains(new Tuple<string, int, int>("~W", x, y - 1)) == false)  // add not wumpus if not found
                            {
                                KB.Add(new Tuple<string, int, int>("~W", x, y - 1));
                            }
                            
                        }
                    }

                    //Inferred additions
                    Console.WriteLine("Knowledge Base for [{0},{1}]",x,y); // display knowledge base of explored
                    Console.WriteLine("--------------------------");
                    for (int i = 0; i < KB.Count; i++)
                    {
                        Console.WriteLine("{0}\n", KB[i]); // displays knowledge base
                    }
                    Console.WriteLine("--------------------------");
                }

                if (Environment[x,y,2] == true) // if breeze is found
                {
                    Inferred.Clear(); // clear inferred list
                    AdjacentTiles = 0;
                    Safespace = 0;
                    if(x + 1 != 4) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~P", x + 1, y)); // add No pit to Inferred List
                        AdjacentTiles++;
                    }
                    if (x - 1 != -1) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~P", x - 1, y)); // add No pit to Inferred List
                        AdjacentTiles++;
                    }
                    if (y + 1 != 4) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~P", x, y + 1)); // add No pit to Inferred List
                        AdjacentTiles++;
                    }
                    if (y - 1 != -1) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~P", x, y - 1)); // add No pit to Inferred List
                        AdjacentTiles++;
                    }
                    for(int i = 0; i < Inferred.Count; i++) // Infer from knowledge base
                    {
                        if (KB.Contains(Inferred[i]))
                        {
                            Safespace++;
                        }
                        else
                        {
                            if (Inferred[i] == new Tuple<string, int, int>("~P", x + 1, y)) // if inferred contains not pit then move
                            {
                                Trouble[0] = x + 1;
                                Trouble[1] = y;
                            }
                            else if (Inferred[i] == new Tuple<string, int, int>("~P", x - 1, y)) // if inferred contains not pit then move
                            {
                                Trouble[0] = x - 1;
                                Trouble[1] = y;
                            }
                            else if (Inferred[i] == new Tuple<string, int, int>("~P", x, y + 1)) // if inferred contains not pit then move
                            {
                                Trouble[0] = x;
                                Trouble[1] = y + 1;
                            }
                            else if (Inferred[i] == new Tuple<string, int, int>("~P", x, y - 1)) // if inferred contains not pit then move
                            {
                                Trouble[0] = x;
                                Trouble[1] = y - 1;
                            }
                        }
                    }
                    if (Safespace == AdjacentTiles - 1) // checks if known adjacent tiles are not pits and make unknown a pit
                    {
                        temp1 = Trouble[0];
                        temp2 = Trouble[1];
                        if (KB.Contains(new Tuple<string, int, int>("P", temp1, temp2)) == false)
                        {
                            KB.Add(new Tuple<string, int, int>("P", temp1, temp2));
                        }
                    }
                }
                // Code below is same as above for stench and wumpus 
                
                if (Environment[x, y, 0] == true) // stench found
                {
                    Inferred.Clear();
                    AdjacentTiles = 0;
                    Safespace = 0;
                    if (x + 1 != 4) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~W", x + 1, y));
                        AdjacentTiles++;
                    }
                    if (x - 1 != -1) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~W", x - 1, y));
                        AdjacentTiles++;
                    }
                    if (y + 1 != 4) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~W", x, y + 1));
                        AdjacentTiles++;
                    }
                    if (y - 1 != -1) // bounds of environment 
                    {
                        Inferred.Add(new Tuple<string, int, int>("~W", x, y - 1));
                        AdjacentTiles++;
                    }
                    for (int i = 0; i < Inferred.Count; i++)
                    {
                        if (KB.Contains(Inferred[i]))
                        {
                            Safespace++;
                        }
                        else
                        {
                            if (Inferred[i] == new Tuple<string, int, int>("~W", x + 1, y))
                            {
                                Trouble[0] = x + 1;
                                Trouble[1] = y;
                            }
                            else if (Inferred[i] == new Tuple<string, int, int>("~W", x - 1, y))
                            {
                                Trouble[0] = x - 1;
                                Trouble[1] = y;
                            }
                            else if (Inferred[i] == new Tuple<string, int, int>("~W", x, y + 1))
                            {
                                Trouble[0] = x;
                                Trouble[1] = y + 1;
                            }
                            else if (Inferred[i] == new Tuple<string, int, int>("~W", x, y - 1))
                            {
                                Trouble[0] = x;
                                Trouble[1] = y - 1;
                            }
                        }
                    }
                    if (Safespace == AdjacentTiles - 1)
                    {
                        temp1 = Trouble[0];
                        temp2 = Trouble[1];
                        if (KB.Contains(new Tuple<string, int, int>("W", temp1, temp2)) == false)
                        {
                            KB.Add(new Tuple<string, int, int>("W", temp1, temp2));
                        }
                    }
                }



                if (goldCarried == false) // if gold is not acquired
                {
                    PossibleTiles.Clear();
                    if (x + 1 != 4) // bounds of environment 
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~W", x + 1, y)) && KB.Contains(new Tuple<string, int, int>("~P", x + 1, y)))
                        {
                            PossibleTiles.Add(new Tuple<int, int>(x + 1, y));
                        }
                    }
                    if (x - 1 != -1) // bounds of environment 
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~W", x - 1, y)) && KB.Contains(new Tuple<string, int, int>("~P", x - 1, y)))
                        {
                            PossibleTiles.Add(new Tuple<int, int>(x - 1, y));
                        }
                    }
                    if (y + 1 != 4) // bounds of environment 
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~W", x, y + 1)) && KB.Contains(new Tuple<string, int, int>("~P", x, y + 1)))
                        {
                            PossibleTiles.Add(new Tuple<int, int>(x, y + 1));
                        }
                    }
                    if (y - 1 != -1) // bounds of environment 
                    {
                        if (KB.Contains(new Tuple<string, int, int>("~W", x, y - 1)) && KB.Contains(new Tuple<string, int, int>("~P", x, y - 1)))
                        {
                            PossibleTiles.Add(new Tuple<int, int>(x, y - 1));
                        }
                    }
                    if (PossibleTiles.Count == 0)
                    {

                    }
                    else if (PossibleTiles.Count == 1)
                    {
                        (int tempx, int tempy) = PossibleTiles[0];
                        x = tempx;
                        y = tempy;
                    }
                    else
                    {
                        UnexploredPossibleTiles.Clear();
                        for (int i = 0; i < PossibleTiles.Count; i++)
                        {
                            (int tempx, int tempy) = PossibleTiles[i];
                            if (Explored[tempx,tempy] == false)
                            {
                                UnexploredPossibleTiles.Add(PossibleTiles[i]);
                            }
                        }
                        if (UnexploredPossibleTiles.Count == 0)
                        {
                            Index = Chosen.Next(PossibleTiles.Count);
                            (int tempx, int tempy) = PossibleTiles[Index];
                            x = tempx;
                            y = tempy;
                        }
                        else if (UnexploredPossibleTiles.Count == 1)
                        {
                            (int tempx, int tempy) = UnexploredPossibleTiles[0];
                            x = tempx;
                            y = tempy;
                        }
                        else
                        {
                            Index = Chosen.Next(UnexploredPossibleTiles.Count);
                            (int tempx, int tempy) = UnexploredPossibleTiles[Index];
                            x = tempx;
                            y = tempy;
                        }
                    }
                    Console.WriteLine("Moved to [{0},{1}]\n", x, y);
                }
                else
                {
                    
                    Escape.Clear();
                    if (x + 1 != 4)
                    {
                        if (Explored[x + 1, y] == true)
                        {
                            Escape.Add(new Tuple<int, int>(x + 1, y));
                        }
                    }
                    if (x - 1 != -1)
                    {
                        if (Explored[x - 1, y] == true)
                        {
                            Escape.Add(new Tuple<int, int>(x - 1, y));
                        }
                    }
                    if (y + 1 != 4)
                    {
                        if (Explored[x, y + 1] == true)
                        {
                            Escape.Add(new Tuple<int, int>(x, y + 1));
                        }
                    }
                    if (y - 1 != -1)
                    {
                        if (Explored[x, y - 1] == true)
                        {
                            Escape.Add(new Tuple<int, int>(x, y - 1));
                        }
                    }
                    if (Escape.Count == 1)
                    {
                        Explored[x, y] = false;
                        (int tempx, int tempy) = Escape[0];
                        x = tempx;
                        y = tempy;
                        Console.WriteLine("Moved to [{0},{1}]\n", x, y);
                    }
                    else
                    {
                        move = false;
                        while (move == false)
                        {
                            if ((y - 1 != -1) && (Escape.Contains(new Tuple<int, int>(x, y - 1))))
                            {
                                
                                    y--;
                                
                                    move = true;
                                
                            }
                            else if ((x - 1 != -1) && (Escape.Contains(new Tuple<int, int>(x - 1, y))))
                            {
                                if (Escape.Contains(new Tuple<int, int>(x - 1, y)))
                                {
                                    x--;
                                    move = true;
                                }
                            }
                            else if ((x + 1 != 4) && (Escape.Contains(new Tuple<int, int>(x + 1, y))))
                            {
                                if (Escape.Contains(new Tuple<int, int>(x + 1, y)))
                                {
                                    x++;
                                    move = true;
                                }
                            }
                            else if ((y + 1 != 4) && (Escape.Contains(new Tuple<int, int>(x, y + 1))))
                            {
                                if (Escape.Contains(new Tuple<int, int>(x, y + 1)))
                                {
                                    y++;
                                    move = true;
                                }
                            }

                            

                        }
                        Console.WriteLine("Moved to [{0},{1}]\n", x, y); // displays movement
                            if (x == 0 && y == 0) // goal state
                            {
                                Console.Write("The Agent has Escaped with the Gold!"); // display ending
                                break; // end program
                            }
                    }
                    
                }

                /* 
                 * 
                 *  count ← a table, where count [c] is the number of symbols in c’s premise
                 *  inferred ← a table,where inferred[s] is initially false for all symbols
                 *  agenda ← a queue of symbols, initially symbols known to be true in KB
                 * 
                 * 
                 * function PL-FC-ENTAILS?(KB, q)
                 * {
                 * while agenda is not empty do
                       p←POP(agenda)
                       if p = q then return true
                       if inferred[p] = false then
                           inferred[p]←true
                           for each clause c in KB where p is in c.PREMISE do
                               decrement count[c]
                               if count[c] = 0 then add c.CONCLUSION to agenda
                   return false}*/



















            }
        }
    }
}