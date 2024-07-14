namespace WarehouseManager.Core.Pages
{
    public static class CompanyInformationLogic
    {
        public static string GetCompanyName()
        {
            return "Aperture Science";
        }

        public static string GetAddresss()
        {
            return "123 Enrichment Center Way, Upper Michigan, USA";
        }

        public static string GetPhoneNumber()
        {
            return "555-9876";
        }

        public static string GetEmail()
        {
            return "contact@aperturescience.com";
        }

        public static string GetRepresentative()
        {
            return "Cave Johnson";
        }

        public static string GetDescription()
        {
            List<string> descriptions = new List<string>{
                "They say great science is built on the shoulders of giants.\nNot here.\nAt Aperture, we do all our science from scratch.\nNo hand holding.",
                "Science isn't about WHY.\nIt's about WHY NOT.\nWhy is so much of our science dangerous?\nWhy not marry safe science if you love it so much.\nIn fact, why not invent a special safety door that won't hit you on the butt on the way out, because you are fired.",
                "All right, I've been thinking.\nWhen life gives you lemons?\nDon't make lemonade.\nMake life take the lemons back! Get mad!\nI don't want your damn lemons.\nWhat am I supposed to do with these?\nDemand to see life's manager!\nMake life rue the day it thought it could give Cave Johnson lemons!\nDo you know who I am?\nI'm the man who's going to burn your house down! With the lemons!\nI'm going to get my engineers to invent a combustible lemon that burns your house down!"
            };

            var rand = new Random();
            return descriptions[rand.Next(descriptions.Count)];
        }
    }
}