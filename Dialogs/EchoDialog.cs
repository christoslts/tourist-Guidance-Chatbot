using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Speech.Synthesis;
using System.Data.SqlClient;
using System.Data;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;
        protected double Tlat { get; set; }
        protected double Tlng { get; set; }


        protected int KM { get; set; }
        protected string msg { get; set; }
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            try
            {
                await context.PostAsync("Hello, What is your name?");
                SpeechSynthesizer sound = new SpeechSynthesizer();  //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Hello, What is your name?"); //Set Reader To Response Output of AIML To Speak
                context.Wait(StartMsg);
            }

            catch (Exception )

            {
                await context.PostAsync("Sorry you make a mistake\n Try again!!");
            }
        }

        public async Task StartMsg(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            await context.PostAsync("Welcome to Cyprus Guidance Bot " + message.Text + "!!! \n What are you interested to learn about? \n -->My Location \n--> Distances \n-->Nearest-places \n-->Cheapest-places \n-->Informations about places ");
            SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
            sound.Speak("Welcome to Cyprus Guidance Bot " + message.Text + " What are you interested to learn about?"); //Set Reader To Response Output of AIML To Speak
            context.Wait(ChooseMsg);

        }
        public async Task redirect(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            await context.PostAsync(" What are you interested to learn about? \n-->My Location \n--> Distances \n-->Nearest-places \n-->Cheapest-places \n-->Informations about places  ");

            context.Wait(ChooseMsg);

        }


        //****the bellow function is for the answer choice******
        public async Task ChooseMsg(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            EditDistance distance = new EditDistance(message.Text);
            String answer = distance.Similarity();


            if (answer == "distances")
            {

                await context.PostAsync("Choose a place:" +
                    "\n -Museums \n -Restaurants \n -Bars ");


            }
            else if (answer == "museums")
            {
                await context.PostAsync("Give your Latitude:");

                context.Wait(DistanceMuseumsLat);

            }
            else if (answer == "restaurants")
            {
                await context.PostAsync("Give your Latitude:");

                context.Wait(DistanceRestaurantsLat);

            }
            else if (answer == "bars")
            {
                await context.PostAsync("Give your latitude:");

                context.Wait(DistanceBarsLat);

            }

            else if (answer == "nearest-places")
            {

                await context.PostAsync("what places do you want? " +
                    "\n -Nearest-Museums \n -Nearest-Restaurants \n -Nearest-Bars");

            }
            else if (answer == "nearest-museums")
            {
                await context.PostAsync("Give your Latitude:");

                context.Wait(NearestMuseumsLat);
            }
            else if (answer == "nearest-restaurants")
            {
                await context.PostAsync("Give your Latitude:");

                context.Wait(NearestRestaurantLat);
            }
            else if (answer == "nearest-bars")
            {
                await context.PostAsync("Give your Latitude:");

                context.Wait(NearestBarsLat);
            }
            else if (answer == "cheapest-places")
            {
                await context.PostAsync("What places do you want to know for?" +
                    "\n -Cheapest-Restaurants \n -Cheapest-Bars");

            }
            else if (answer == "cheapest-restaurants")
            {
                await context.PostAsync("Please provide a district:");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Please provide a city:");
                context.Wait(CheapestRestaurants);
            }
            else if (answer == "cheapest-bars")
            {
                await context.PostAsync("Please provide a district:");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Please provide a city:");
                context.Wait(CheapestBars);
            }
            else if (answer == "informations")
            {
                await context.PostAsync("what places information do you want for? " +
                   "\n -Museums-Info \n -Restaurants-Info \n -Bars-Info");
                //SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                //sound.Speak("what places information do you want for? " +
                //   "\n Museums-Info \n Restaurants-Info \n Bars-Info");
            }
            else if (answer == "museums-info")
            {
                await context.PostAsync("Please provide a district:");
                //SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                //sound.Speak("Please provide a city:");
                context.Wait(MuseumsInfo);

            }
            else if (answer == "restaurants-info")
            {
                await context.PostAsync("Please provide a district:");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Please provide a city:");
                context.Wait(RestaurantsInfo);

            }
            else if (message.Text == "bars-info")
            {
                await context.PostAsync("Please provide a district:");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Please provide a city:");
                context.Wait(BarsInfo);

            }
            else if (answer == "location")
            {
                await context.PostAsync("Please provide your current town: ");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Please provide your town:");
                context.Wait(LocationsCoordinates);
            }

            else
            {
                await context.PostAsync("Sorry i couldn't understand!! \n Please select one of the following options:\n-My location \n-Distances \n-Nearest places \nCheapest-places  \n-Informations");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Sorry i couldn't understand. Please  select one of the following options: ");
                context.Wait(ChooseMsg);

            }


        }
        //****distances LAT and LNG MILES FUNCTIONS
        //LAT LNG MILES FUNCTION FOR MUSEUMS
        public async Task DistanceMuseumsLat(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;

            try
            {
                Tlat = Convert.ToDouble(message.Text);
                await context.PostAsync("Give your longitude:");
                context.Wait(DistanceMuseumsLng);
            }

            catch (Exception )

            {
                await context.PostAsync("Please enter your coordinates!! \n Do you want to learn them?");
                context.Wait(redirect);
            }
        }
        public async Task DistanceMuseumsLng(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            Tlng = Convert.ToDouble(message.Text);
            await context.PostAsync("Give the maximum range from you:");

            context.Wait(DistanceMuseumsMiles);
        }
        public async Task DistanceMuseumsMiles(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            KM = Convert.ToInt32(message.Text);
            await context.PostAsync("Do you like to see the results?");

            context.Wait(DistanceMuseums);
        }
        //LAT LNG MILES FOR RESTAURANTS

        public async Task DistanceRestaurantsLat(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            try
            {
                Tlat = Convert.ToDouble(message.Text);
                await context.PostAsync("Give your longitude:");
                context.Wait(DistanceRestaurantsLng);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                await context.PostAsync("Please enter your coordinates!! \n Do you like to learn them?");
                context.Wait(redirect);
            }
        }
        public async Task DistanceRestaurantsLng(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            Tlng = Convert.ToDouble(message.Text);
            await context.PostAsync("Give the maximum range from you:");
            context.Wait(DistanceRestaurantsKilometers);
        }
        public async Task DistanceRestaurantsKilometers(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            KM = Convert.ToInt32(message.Text);
            await context.PostAsync("Do you like to see the results?");

            context.Wait(DistanceRestaurants);
        }
        //LAT LNG MILES FOR BARS

        public async Task DistanceBarsLat(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            try
            {

                Tlat = Convert.ToDouble(message.Text);
                await context.PostAsync("Give your longitude:");

                context.Wait(DistanceBarsLng);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                await context.PostAsync("Please enter your coordinates!!\n Do you like to learn them? ");
                context.Wait(redirect);
            }
        }
        public async Task DistanceBarsLng(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            Tlng = Convert.ToDouble(message.Text);
            await context.PostAsync("Give the maximum range from you:");

            context.Wait(DistanceBarsKilometers);
        }
        public async Task DistanceBarsKilometers(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            KM = Convert.ToInt32(message.Text);
            await context.PostAsync("Do you like to see the results?");
            context.Wait(DistanceBars);
        }


        //****Distance museums***
        public async Task DistanceMuseums(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            if (message.Text.ToLower().Equals("no"))
            {
                await context.PostAsync("I hope i was helpful \n Enjoy the rest!!!");
                context.Wait(redirect);
            }

            else if (message.Text == "yes")
            {
                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd = new SqlCommand("dbo.CalculateDistance", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@lat", Tlat));
                cmd.Parameters.Add(new SqlParameter("@lng", Tlng));
                cmd.Parameters.Add(new SqlParameter("@kilometers", KM));
                SqlDataReader reader = cmd.ExecuteReader();

                await context.PostAsync("Here is my Recomentations.. \n Based on your location distance..");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Here is my Recomentations.. \n Based on your location distance..");

                while (reader.Read())

                {

                    Double kilometer = Math.Truncate(100 * Convert.ToDouble(reader[4])) / 100;
                    msg = "Museum Name: " + reader[0].ToString() + "\nCity:  " + reader[1].ToString() +
                         "\nClick here to see the map:-->  http://maps.google.com/maps?z=18&q=" + reader[2] + "," + reader[3] + "\nDistances:  " + kilometer.ToString() + " Kilometers ";

                    await context.PostAsync(msg);
                }

                connection.Close();
                reader.Close();

                await Task.Delay(5000);
                await context.PostAsync("Do you want to see other distances from other location?");

                //if (String.IsNullOrEmpty(msg))
                //{

                //    msg = "There is NO available museums near to you";
                //    await context.PostAsync(msg);
                //    await Task.Delay(10000);
                //    await context.PostAsync("Do you want to try again?");
                //    context.Wait(redirect);

                //}


            }

        }
        //*****distance for restaurants*****
        public async Task DistanceRestaurants(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            if (message.Text.ToLower().Equals("no"))
            {
                await context.PostAsync("I hope i was helpful \n Enjoy the rest!!");
            }



            else if (message.Text.ToUpper().Equals("YES"))
            {

                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd = new SqlCommand("dbo.RestaurantDistance", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@lat", Tlat));
                cmd.Parameters.Add(new SqlParameter("@lng", Tlng));
                cmd.Parameters.Add(new SqlParameter("@kilometers", KM));
                SqlDataReader reader = cmd.ExecuteReader();
                await context.PostAsync("Here is my Recomentations..\n Based on your location distance..");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Here is my Recomentations.. \n Based on your location distance..");
                while (reader.Read())
                {
                    Double kilometer = Math.Truncate(100 * Convert.ToDouble(reader[4])) / 100;
                    msg = "Restaurant Name: " + reader[0].ToString() + "\nCity:  " + reader[1].ToString() +
                          "\nClick here to see the map:-->  http://maps.google.com/maps?z=18&q=" + reader[2] + "," + reader[3] + "\nDistance:  " + kilometer.ToString() + " Kilometers ";

                    await context.PostAsync(msg);
                }

                connection.Close();
                reader.Close();
                await Task.Delay(5000);
                await context.PostAsync("Do you any other nearest restaurants?");

                if (String.IsNullOrEmpty(msg))
                {

                    msg = "There is NO available restaurants near to you";
                    await context.PostAsync(msg);
                    await Task.Delay(5000);
                    await context.PostAsync("Do you want to try again?");
                    context.Wait(redirect);

                }
            }
        }
        //*****distance bars*****

        public async Task DistanceBars(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            if (message.Text.ToLower().Equals("no"))
            {
                await context.PostAsync("I hope i was helpful \n Enjoy the rest!!");
                context.Wait(redirect);
            }

            else if (message.Text.ToUpper().Equals("YES"))
            {

                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();


                cmd = new SqlCommand("dbo.BarsDistance", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@lat", Tlat));
                cmd.Parameters.Add(new SqlParameter("@lng", Tlng));
                cmd.Parameters.Add(new SqlParameter("@kilometers", KM));
                SqlDataReader reader = cmd.ExecuteReader();
                await context.PostAsync("Here is my Recomentations..\n Based on your location distance..");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Here is my Recomentations, Based on your location distance");
                while (reader.Read())
                {
                    Double kilometer = Math.Truncate(100 * Convert.ToDouble(reader[4])) / 100;
                    msg = "Bar Name: " + reader[0].ToString() + "\nCity:  " + reader[1].ToString() +
                           "\nClick here to see the map:-->  http://maps.google.com/maps?z=18&q=" + reader[2] + "," + reader[3] + "\nDistances:  " + kilometer.ToString() + " Kilometers ";


                    await context.PostAsync(msg);
                }

                connection.Close();
                reader.Close();

                //if (String.IsNullOrEmpty(msg))
                //{

                //    msg = "There is NO available bars near to you";
                //    await context.PostAsync(msg);
                //    await Task.Delay(1000);
                //}
                await context.PostAsync("Do you want to try again?");
                context.Wait(redirect);


            }
        }

        //*******LATITUDE AND LONGITUDE For NEAREST museums*****

        public async Task NearestMuseumsLat(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            try
            {

                Tlat = Convert.ToDouble(message.Text);
                await context.PostAsync("Give your longitude:");
                context.Wait(NearestMuseumsLng);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                await context.PostAsync("Please enter your coordinates!! \n Do you like to learn them?");
                context.Wait(redirect);
            }


        }
        public async Task NearestMuseumsLng(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            Tlng = Convert.ToDouble(message.Text);
            await context.PostAsync("Do you like to see the map?");
            context.Wait(NearestMuseums);
        }
        //*****Latitude and Longitude for restaurants*****
        public async Task NearestRestaurantLat(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            try
            {
                Tlat = Convert.ToDouble(message.Text);
                await context.PostAsync("Give your longitude:");
                context.Wait(NearestRestaurantLng);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                await context.PostAsync("Please enter your coordinates!! \n Do you like to learn them?");
                context.Wait(redirect);
            }

        }
        public async Task NearestRestaurantLng(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            Tlng = Convert.ToDouble(message.Text);
            await context.PostAsync("Do you like to see the map?");
            context.Wait(NearestRestaurants);
        }
        //****Latitude and Longitude for BARS*****
        public async Task NearestBarsLat(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {

            var message = await answer;
            try
            {
                Tlat = Convert.ToDouble(message.Text);
                await context.PostAsync("Give your longitude:");
                context.Wait(NearestBarsLng);
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                await context.PostAsync("Please enter your coordinates!! \n Do you like to learn them?");
                context.Wait(redirect);
            }

        }
        public async Task NearestBarsLng(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;
            Tlng = Convert.ToDouble(message.Text);
            await context.PostAsync("do you like to see the map?");
            //SpeechSynthesizer reader = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
            //reader.Speak("do you like to see the map?");
            context.Wait(NearestBars);
        }

        //***NEAREST PLACES FUNCTION***

        public async Task NearestMuseums(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helpful \n Enjoy the rest!!");
            }

            else if (message.Text.ToUpper().Equals("YES"))
            {



                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();

                connection.Open();
                cmd = new SqlCommand("get_nearest_Museums", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@lat", Tlat));
                cmd.Parameters.Add(new SqlParameter("@lng", Tlng));
                SqlDataReader reader = cmd.ExecuteReader();
                await context.PostAsync("Here is my Recomentations..");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Here is my Recomentations about nearest museums");

                while (reader.Read())
                {
                    string msg = "Museum Name: " + reader[1] + "\nCity: " + reader[2] + "\nLocation: Click here--> http://maps.google.com/maps?z=18&q=" + reader[3] + "," + reader[4];

                    await context.PostAsync(msg);
                }

                connection.Close();
                reader.Close();

                await Task.Delay(6000);
                await context.PostAsync("Do you want other nearest museums?");



            }


        }

        public async Task NearestRestaurants(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helpful \n Enjoy the rest!!");

            }


            else
            {
                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();


                connection.Open();
                cmd = new SqlCommand("get_nearest_Restaurants", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@lat", Tlat);
                cmd.Parameters.AddWithValue("@lng", Tlng);
                SqlDataReader reader = cmd.ExecuteReader();
                await context.PostAsync("Here is my Recomentations about nearest restaurants");
                SpeechSynthesizer Sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                Sound.Speak("Here is my recomontations about nearest restaurants");
                while (reader.Read())
                {
                    string msg = "Restaurant Name: " + reader[1] + "\nCity:" + reader[2] + "\n Location: Click Here--> http://maps.google.com/maps?z=18&q=" + reader[3] + "," + reader[4];

                    await context.PostAsync(msg);
                }

                connection.Close();
                reader.Close();

            }
            await Task.Delay(10000);
            await context.PostAsync("Do you want other nearest restaurants?");
            SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
            sound.Speak("Do you want other nearest restaurants");

        }


        public async Task NearestBars(IDialogContext context, IAwaitable<IMessageActivity> answer)
        {
            var message = await answer;

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helpful \n Enjoy the rest!!");
                SpeechSynthesizer Sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                Sound.Speak("I hope i was helpful \n Enjoy the rest");
            }



            else
            {

                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                connection.Open();
                SqlCommand cmd = new SqlCommand("dbo.get_nearest_Bars", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@lat", Tlat));
                cmd.Parameters.Add(new SqlParameter("@lng", Tlng));
                SqlDataReader reader = cmd.ExecuteReader();
                await context.PostAsync("Here is my Recomentations about nearest bars");
                SpeechSynthesizer Sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                Sound.Speak("Here is my recomontations about nearest bars");
                //if (reader.Read())
                //{
                //    string msg = "https://www.google.com.qa/maps/d/embed?mid=13p3PyTyKnTRKKQ4_uiwm8f9WjFjGauvC";
                //    await context.PostAsync(msg);
                //}
                while (reader.Read())
                {

                    string msg = "Bar Name: " + reader[1] + "\nCity: " + reader[2] + " \n Location: Click Here--> http://maps.google.com/maps?z=18&q=" + reader[3] + "," + reader[4];

                    await context.PostAsync(msg);
                }

                connection.Close();
                reader.Close();

            }

            await Task.Delay(10000);
            await context.PostAsync("Do you want other nearest bars?");
            SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
            sound.Speak("Do you want other nearest bars");


        }




        //*********INFORMATIONS ABOUT PLACES*********


        public async Task MuseumsInfo(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            string loc = "";
            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helpful.\n Enjoy the rest!!!");

            }


            else if (loc != null)
            {
                EditDistance distance = new EditDistance(message.Text);
                loc = distance.Location();


                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd = new SqlCommand("SELECT * FROM Museum WHERE City = '" + loc.ToLower() + "'", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    await context.PostAsync("The district that you provide does not exist. \n Please try again!!");
                    context.Wait(CheapestRestaurants);
                }
                else
                {

                    await context.PostAsync("Here is my recommendations.....");
                    SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                    sound.Speak("Here is my recomontations");
                    while (reader.Read())
                    {

                        String msg = " Museum Name: " + reader[1].ToString() + "\nCity :" + reader[2].ToString() + "\nAddress: " + reader[3].ToString() + "\nPhone: " + reader[4].ToString();


                        await context.PostAsync(msg);

                    }
                    await Task.Delay(6000);
                    await context.PostAsync("Do you want any other district??");
                    context.Wait(MuseumsInfo);
                }
            }


        }


        public async Task RestaurantsInfo(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            string loc = "";

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("Have a nice day!!!");

            }
            else if (message.Text.ToLower() == "yes")
            {
                context.Wait(RestaurantsInfo);
            }

            else if (loc != null)

            {

                EditDistance distance = new EditDistance(message.Text);
                loc = distance.Location();


                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd = new SqlCommand("SELECT * FROM Restaurant WHERE City = '" + loc.ToLower() + "'", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    await context.PostAsync("The district that you provide does not exist \n Please try again!!");
                    context.Wait(CheapestRestaurants);
                }
                else
                {

                    await context.PostAsync("Here is my recommendations.....");

                    while (reader.Read())
                    {

                        String msg = " Restaurant Name: " + reader[1].ToString() + "\nAddress :" + reader[2].ToString() + "\nCity: " + reader[3].ToString() + "\nPhone: " + reader[4].ToString() + "\n Rate:" + reader[5].ToString() + "/5";


                        await context.PostAsync(msg);

                    }



                    await Task.Delay(6000);
                    await context.PostAsync("Do you want any other district??");
                }



            }

        }
        //search for bars
        public async Task BarsInfo(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            string loc = "";

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helful \n Enjoy the rest!!!");

                context.Wait(redirect);
            }

            else if (loc != null)
            {
                EditDistance distance = new EditDistance(message.Text);
                loc = distance.Location();

                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd = new SqlCommand("SELECT * FROM Bars WHERE City = '" + loc.ToUpper() + "'", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                await context.PostAsync("Here is my recommendations.....");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Here is my recomontations");
                while (reader.Read())
                {

                    String msg = " Bar Name: " + reader[1].ToString() + "\nAddress :" + reader[2].ToString() + "\nCity: " + reader[3].ToString() + "\nPhone: " + reader[4].ToString() + "\n Rate:" + reader[5].ToString() + "/5";


                    await context.PostAsync(msg);

                }
                await Task.Delay(6000);
                await context.PostAsync("Do you want any other district??");
                context.Wait(BarsInfo);


            }

        }
        //current location function
        public async Task LocationsCoordinates(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {


            var message = await argument;
            string loc = "";

            if (message.Text == "no")
            {
                await context.PostAsync("Have a nice day!!!");

            }
            else if (message.Text == "yes")
            {

                await context.PostAsync("Choose one of the two categories: \n--> Distances \n-->Nearest places ");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("Choose one of the two categories:");
                context.Wait(ChooseMsg);

            }


            else if (message.Text != null)

            {
                EditDistance distance = new EditDistance(message.Text);
                loc = distance.Location();

                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM Location WHERE Town = '" + message.Text.ToLower() + "'", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    await context.PostAsync("Here is your Coordinates\n Use them when it requires.....");
                    SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                    sound.Speak("Here is your Coordinates\n Use them when it requires");

                    String msg = "Your Latitude is: '" + reader[3] + "' \n Your Longitude is: '" + reader[4];
                    await context.PostAsync(msg);


                    await Task.Delay(3000);

                    await context.PostAsync("Do you want use them now?");
                    SpeechSynthesizer sound1 = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                    sound1.Speak("Do you want use them now?");
                }

                else if (!reader.Read())
                {

                    await context.PostAsync("Sorry you provide false Town!!\n Please try again");
                    context.Wait(LocationsCoordinates);
                }
            }

        }
        //cheapest bars
        public async Task CheapestBars(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            string loc = "";

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helful \n Enjoy the rest!!!");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //Add System.Speech Reference First In Order To Creating It.
                sound.Speak("I hope i was helful \n Enjoy the rest");
                context.Wait(redirect);
            }

            else if (loc != null)

            {
                EditDistance distance = new EditDistance(message.Text);
                loc = distance.Location();


                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                connection.Open();

                cmd = new SqlCommand(" select * from Bars Where City ='" + loc.ToUpper() + "' ORDER BY Price_rate ASC  ", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    await context.PostAsync("The district that you provide does not exist. \n Please try again!!");
                    context.Wait(CheapestRestaurants);
                }
                else
                {

                    await context.PostAsync("Here is the cheapest bars in order.....");

                    while (reader.Read())
                    {

                        String msg = " Bar Name: " + reader[1].ToString() + "\nAddress :" + reader[2].ToString() + "\nCity: " +
                            reader[3].ToString() + "\nPricing rate: " + reader[6].ToString() + "/5";


                        await context.PostAsync(msg);

                    }
                    await Task.Delay(4000);
                    await context.PostAsync("Do you want any other district??");
                    context.Wait(CheapestBars);
                }

            }

        }
        //cheapest restaurants
        public async Task CheapestRestaurants(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            string loc = "";

            if (message.Text.ToLower() == "no")
            {
                await context.PostAsync("I hope i was helful \n Enjoy the rest!!!");
                SpeechSynthesizer sound = new SpeechSynthesizer(); //sound reference
                sound.Speak("I hope i was helful \n Enjoy the rest");
                context.Wait(redirect);
            }

            else if (loc != null)
            {
                EditDistance distance = new EditDistance(message.Text);

                loc = distance.Location();


                SqlConnection connection = new SqlConnection("Data Source=CHRISTOS\\SQLEXPRESS;Initial Catalog=Tourist;Integrated Security=True");

                SqlCommand cmd = new SqlCommand();
                connection.Open();
                cmd = new SqlCommand(" select * from Restaurant Where City ='" + loc.ToUpper() + "' ORDER BY Price_rate ASC ", connection);

                SqlDataReader reader = cmd.ExecuteReader();


                if (!reader.Read())
                {
                    await context.PostAsync("The district that you provide does not exist. \n Please try again!!");
                    SpeechSynthesizer sound = new SpeechSynthesizer(); //sound reference
                    sound.Speak("The district that you provide does not exist. \n Please try again");
                    context.Wait(CheapestRestaurants);
                }
                else
                {


                    await context.PostAsync("Here is the cheapest restaurants in order.....");
                    SpeechSynthesizer sound = new SpeechSynthesizer(); //sound reference
                    sound.Speak("Here is the cheapest restaurants in order");
                    while (reader.Read())
                    {

                        String msg = " Restaurant Name: " + reader[1].ToString() + "\nCity: " + reader[2].ToString() + "\nAddress: " +
                            reader[3].ToString() + "\nPricing rate: " + reader[6].ToString() + "/5";
                        await context.PostAsync(msg);

                    }

                    await Task.Delay(5000);
                    await context.PostAsync("Do you want any other district??");
                    SpeechSynthesizer sound2 = new SpeechSynthesizer(); //sound reference
                    sound2.Speak("Do you want any other district??");
                    context.Wait(CheapestRestaurants);
                }
            }
        }


    }

}
