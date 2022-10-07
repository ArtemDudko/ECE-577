// C# implementation of above approach
using System;
using System.Collections;
using System.Collections.Generic;

class GFG
{

    // graph
    static List<List<int>> graph = new List<List<int>>();

    // map to store cost of edges
    static Dictionary<Tuple<int, int>, int> cost = new Dictionary<Tuple<int, int>, int>();

    // returns the minimum cost in a vector( if
    // there are multiple goal states)
    static List<int> uniform_cost_search(List<int> goal, int start)
    {
        // minimum cost upto
        // goal state from starting
        // state
        List<int> answer = new List<int>();

        // create a priority queue
        List<Tuple<int, int>> frontier = new List<Tuple<int, int>>();

        // set the answer vector to max value
        for (int i = 0; i < goal.Count; i++)
            answer.Add(int.MaxValue);

        // insert the starting index
        frontier.Add(new Tuple<int, int>(0, start));

        // map to store visited node
        Dictionary<int, int> visited = new Dictionary<int, int>();

        // count
        int count = 0;

        // while the queue is not empty
        while (frontier.Count > 0)
        {

            // get the top element of the
            // priority queue
            Tuple<int, int> q = frontier[0];
            Tuple<int, int> p = new Tuple<int, int>(-q.Item1, q.Item2);

            // pop the element
            frontier.RemoveAt(0);


            // check if the element is part of
            // the goal list
            if (goal.Contains(p.Item2))
            {

                // get the position
                int index = goal.IndexOf(p.Item2);

                // if a new goal is reached
                if (answer[index] == int.MaxValue)
                    count++;

                // if the cost is less
                 if (answer[index] > p.Item1)
                    answer[index] = p.Item1;

                // pop the element
                frontier.RemoveAt(0);

                // if all goals are reached
                if (count == goal.Count)
                    return answer;
            }

            // check for the non visited nodes
            // which are adjacent to present node
            if (!visited.ContainsKey(p.Item2))
                for (int i = 0; i < graph[p.Item2].Count; i++)
                {

                    // value is multiplied by -1 so that
                    // least priority is at the top
                    frontier.Add(new Tuple<int, int>((p.Item1 + (cost.ContainsKey(new Tuple<int, int>(p.Item2, graph[p.Item2][i])) ? cost[new Tuple<int, int>(p.Item2, graph[p.Item2][i])] : 0)) * -1,
                    graph[p.Item2][i]));
                }

            // mark as visited
            visited[p.Item2] = 1;
        }

        return answer;
    }

    // main function
    public static void Main(params string[] args)
    {
        // create the graph
        graph = new List<List<int>>();

        for (int i = 0; i <= 19; i++)
        {
            graph.Add(new List<int>());
        }

         // add edge and cost
        graph[0].Add(1);    cost[new Tuple<int, int>(0, 1)] = 75;    // Arad -> Zerind
        graph[0].Add(2);    cost[new Tuple<int, int>(0, 2)] = 140;    // Arad -> Sibiu
        graph[0].Add(3);    cost[new Tuple<int, int>(0, 3)] = 118;    // Arad -> Timisoara
        graph[1].Add(0);    cost[new Tuple<int, int>(1, 0)] = 75;    // Zerind -> Arad
        graph[1].Add(4);    cost[new Tuple<int, int>(1, 4)] = 71;    // Zerind -> Oradea
        graph[2].Add(0);    cost[new Tuple<int, int>(2, 0)] = 140;    // Sibiu -> Arad
        graph[2].Add(5);    cost[new Tuple<int, int>(2, 5)] = 99;    // Sibiu -> Fagaras
        graph[2].Add(6);    cost[new Tuple<int, int>(2, 6)] = 80;    // Sibiu -> Rimnicu Vilcea
        graph[3].Add(0);    cost[new Tuple<int, int>(3, 0)] = 118;    // Timisoara -> Arad
        graph[3].Add(7);    cost[new Tuple<int, int>(3, 7)] = 111;    // Timisoara -> Lugoj
        graph[4].Add(1);    cost[new Tuple<int, int>(4, 1)] = 71;    // Oradea -> Zerand
        graph[4].Add(2);    cost[new Tuple<int, int>(4, 2)] = 151;    // Oradea -> Sibiu
        graph[5].Add(2);    cost[new Tuple<int, int>(5, 2)] = 99;    // Fagaras -> Sibiu
        graph[5].Add(8);    cost[new Tuple<int, int>(5, 8)] = 211;    // Fagaras -> Bucharest
        graph[6].Add(2);    cost[new Tuple<int, int>(6, 2)] = 80;    // Rimnicu Vilcea -> Sibiu
        graph[6].Add(13);   cost[new Tuple<int, int>(6, 13)] = 146;    // Rimnicu Vilcea -> Craiova
        graph[6].Add(9);    cost[new Tuple<int, int>(6, 9)] = 97;    // Rimnicu Vilcea -> Pitesti
        graph[7].Add(3);    cost[new Tuple<int, int>(7, 3)] = 111;    // Lugoj -> Timisoara
        graph[7].Add(10);   cost[new Tuple<int, int>(7, 10)] = 70;    // Lugoj -> Mehadia
        graph[8].Add(5);    cost[new Tuple<int, int>(8, 5)] = 211;    // Bucharest -> Fagaras
        graph[8].Add(9);    cost[new Tuple<int, int>(8, 9)] = 111;    // Bucharest -> Pitesti
        graph[8].Add(12);   cost[new Tuple<int, int>(8, 12)] = 90;    // Bucharest -> Giurgiu
        graph[8].Add(11);   cost[new Tuple<int, int>(8, 11)] = 85;    // Bucharest -> Urziceni
        graph[9].Add(6);    cost[new Tuple<int, int>(9, 6)] = 97;    // Pitesti -> Rimnicu Vilcea
        graph[9].Add(13);   cost[new Tuple<int, int>(9, 13)] = 138;    // Pitesti -> Craiova
        graph[9].Add(8);    cost[new Tuple<int, int>(9, 8)] = 101;    // Pitesti -> Bucharest
        graph[10].Add(7);   cost[new Tuple<int, int>(10, 7)] = 70;    // Mehadia -> Lugoj
        graph[10].Add(19);  cost[new Tuple<int, int>(10, 19)] = 75;    // Mehadia -> Drobeta
        graph[11].Add(8);   cost[new Tuple<int, int>(11, 8)] = 85;    // Urziceni -> Bucharest
        graph[11].Add(15);  cost[new Tuple<int, int>(11, 15)] = 98;    // Urziceni -> Hirsova
        graph[11].Add(14);  cost[new Tuple<int, int>(11, 14)] = 142;    // Urziceni -> Vaslui
        graph[12].Add(8);   cost[new Tuple<int, int>(12, 8)] = 90;    // Giurgiu -> Bucharest
        graph[13].Add(19);  cost[new Tuple<int, int>(13, 19)] = 120;    // Craiova -> Drobeta
        graph[13].Add(6);   cost[new Tuple<int, int>(13, 6)] = 146;    // Craiova -> Rimnicu Vilcea
        graph[13].Add(9);   cost[new Tuple<int, int>(13, 9)] = 138;    // Craiova -> Pitesti
        graph[14].Add(11);  cost[new Tuple<int, int>(14, 11)] = 142;    // Vaslui -> Urziceni
        graph[14].Add(16);  cost[new Tuple<int, int>(14, 16)] = 92;    // Vaslui -> Iasi
        graph[15].Add(11);  cost[new Tuple<int, int>(15, 11)] = 98;    // Hirsova -> Urziceni
        graph[15].Add(17);  cost[new Tuple<int, int>(15, 17)] = 86;    // Hirsova -> Eforie
        graph[16].Add(14);  cost[new Tuple<int, int>(16, 14)] = 92;    // Iasi -> Vaslui
        graph[16].Add(18);  cost[new Tuple<int, int>(16, 18)] = 87;    // Iasi -> Neamt
        graph[17].Add(15);  cost[new Tuple<int, int>(17, 15)] = 86;    // Eforie -> Hirsova
        graph[18].Add(16);  cost[new Tuple<int, int>(18, 16)] = 87;    // Neamt -> Iasi
        graph[19].Add(10);  cost[new Tuple<int, int>(19, 10)] = 75;    // Drobeta -> Mehadia
        graph[19].Add(13);  cost[new Tuple<int, int>(19, 13)] = 120;    // Drobeta -> Craiova


       

        // goal state
        List<int> goal = new List<int>();

        // set the goal
        goal.Add(8); // Goal = Bucharest (8)
        goal.Add(8); // Goal
        goal.Add(8); // Goal

        // get the answer
        List<int> answer = uniform_cost_search(goal, 0);

        // print the answer
        Console.Write("Minimum cost from Arad to Bucharest is = " + answer[0]);

    }
}