using edu.stanford.nlp.ie.crf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    public class EditDistance
    {
        String answer = "";


        public EditDistance(string a)
        {

            answer = a.ToString();

        }

        public string Location()
        {

            //where the nlp library is stored
            var source = @"C:\Users\chris\Downloads\stanford-ner-2018-02-27\stanford-ner-2018-02-27\classifiers\english.all.3class.distsim.crf.ser.gz";

            //declare a location string that use later of for identify words such as (Nicosia,Limassol)
            const string location = "LOCATION";

            var classifier = CRFClassifier.getClassifierNoExceptions(source);
            int a = 0;
            String[] words_array = answer.Split(' ');
            String output = "";
            String[] Array = classifier.classifyToString(answer).Split(' ');
            while (a < words_array.Length)
            {
                output = Array[a].ToString();

                if (output.Contains(location))
                {
                    Console.WriteLine(Array[a]);
                    return words_array[a];

                }
                else
                {
                    return words_array[a];

                }


            }

            return null;


        }
        // check the answer that the user enter and calculate with the distance function
        public string Similarity()
        {

            int a;

            String check_answer = "";
            String[] words_array = answer.Split(' ');

            for (a = 0; a < words_array.Length; a++)
            {
                check_answer = words_array[a].ToString().ToLower();

                if (Distance(check_answer, "nearest places") <= 2 || Distance(check_answer, "nerest-places") <= 1 ||
                    Distance(check_answer, "nearestplaces") <= 1 || Distance(check_answer, "nearest") <= 2)

                {
                    return "nearest places";
                }
                if (Distance(check_answer, "nearest-museums") <= 2 || Distance(check_answer, "nerest-musems") <= 2 || Distance(check_answer, "nearestmusums") <= 2
                    || Distance(check_answer, "nearestmuseums") <= 1)
                {

                    return "nearest-museums";
                }
                if (Distance(check_answer, "nearest-resturants") <= 2 || Distance(check_answer, "nerest-restarants") <= 1 || Distance(check_answer, "nerestrestarants") <= 2
                   || Distance(check_answer, "nearestrestaurants") <= 1)
                {

                    return "nearest-restaurants";
                }
                if (Distance(check_answer, "nearest-bars") <= 2 || Distance(check_answer, "nerest-bar") <= 2 || Distance(check_answer, "nearestbar") <= 2
                   || Distance(check_answer, "nearestbars") <= 1)
                {

                    return "nearest-bars";
                }

                if (Distance(check_answer, "museums-info") <= 2 || Distance(check_answer, "musems-info") <= 1 || Distance(check_answer, "musumsinfo") <= 2
                || Distance(check_answer, "museumsinfo") <= 1)
                {

                    return "museums-info";
                }
                if (Distance(check_answer, "restaurants-info") <= 2 || Distance(check_answer, "restauratns-info") <= 1 || Distance(check_answer, "restaurantsinfo") <= 1
                    || Distance(check_answer, "restaurant-info") <= 1 || Distance(check_answer, "restarantingo") <= 2)
                {

                    return "restaurants-info";
                }
                if (Distance(check_answer, "bars-info") <= 2 || Distance(check_answer, "bats-info") <= 1 || Distance(check_answer, "barsinfo") <= 1
                    || Distance(check_answer, "bar-info") <= 1 || Distance(check_answer, "barsingo") <= 2)
                {

                    return "bars-info";
                }
                if (Distance(check_answer, "cheapest places") <= 2 || Distance(check_answer, "cheapestplaces") <= 1 || Distance(check_answer, "chepest-places") <= 1
                    || Distance(check_answer, "cheapstplaces") <= 2 || Distance(check_answer, "cheapest") <= 2)
                {

                    return "cheapest places";
                }
                if (Distance(check_answer, "cheapest-bars") <= 2 || Distance(check_answer, "cheapestbars") <= 1 || Distance(check_answer, "chepest-bars") <= 1
                    || Distance(check_answer, "cheapstbars") <= 2 || Distance(check_answer, "cheapest bars") <= 1)
                {

                    return "cheapest-bars";
                }
                if (Distance(check_answer, "cheapest-restaurants") <= 2 || Distance(check_answer, "cheapest-restarant") <= 1 ||
                    (Distance(check_answer, "cheapest restaurants") <= 1 || Distance(check_answer, "chepest-restaurants") <= 2))
                {
                    return "cheapest-restaurants";
                }
                if (Distance(check_answer, "restaurants") <= 2 || Distance(check_answer, "restarants") <= 1
                    || Distance(check_answer, "retsaurant") <= 1 || Distance(check_answer, "restaurant") <= 1)
                {
                    return "restaurants";
                }
                if (Distance(check_answer, "museums") <= 2 || Distance(check_answer, "musems") <= 1)
                {
                    return "museums";
                }
                if (Distance(check_answer, "informations") <= 2 || Distance(check_answer, "information") <= 1
                    || Distance(check_answer, "infromation") <= 2 || Distance(check_answer, "info") <= 2)
                {
                    return "informations";
                }
                if (Distance(check_answer, "distances") <= 2 || Distance(check_answer, "distance") <= 1 || Distance(check_answer, "ditsances") <= 1)
                {
                    return "distances";
                }
                if (Distance(check_answer, "location") <= 2 || Distance(check_answer, "locations") <= 1 || Distance(check_answer, "locaton") <= 1)
                {
                    return "location";
                }
                if (Distance(check_answer, "bars") <= 2 || Distance(check_answer, "bar") <= 1 || Distance(check_answer, "bras") <= 1)
                {
                    return "bars";
                }

            }


            return null;


        }



        //function that calculate the distance between the words 
        private static int Distance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (String.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }


    }
}