using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = AddHeader(invoice);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = CalculateAmount(play, perf);

                // add extra credit for every ten comedy attendees
                volumeCredits = AddExtraCredit(play, volumeCredits, perf);
                totalAmount += thisAmount;

                // print line for this order
                result += AddPlay(cultureInfo, play, perf, thisAmount);
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static string AddHeader(Invoice invoice)
        {
            return string.Format("Statement for {0}\n", invoice.Customer);
        }

        private static string AddHeaderHTML(Invoice invoice)
        {
             return string.Format("<h1>Statement for {0}</h1>\n", invoice.Customer);
        }

        private static string AddPlay(CultureInfo cultureInfo, Play play, Performance perf, int thisAmount)
        {
            return string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
        }

        private static string AddPlayHTML(CultureInfo cultureInfo, Play play, Performance perf, int thisAmount)
        {
             return string.Format(cultureInfo, "  <tr><td>{0}</td> <td>{1}</td> <td>{2:C}</td></tr>)\n", play.Name, perf.Audience, Convert.ToDecimal(thisAmount / 100));
        }




        private static int CalculateAmount(Play play, Performance perf)
        {
            switch (play.Type)
            {
                case "tragedy":
                    return CalculateAmountTragedy(perf);
                case "comedy":
                    return CalculateAmountComedy(perf);
                default:
                    throw new Exception("unknown type: " + play.Type);
            }
        }

        private static int AddExtraCredit(Play play, int volumeCredits, Performance perf)
        {
            // add volume credits
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            
            if ("comedy" == play.Type) volumeCredits += (int) Math.Floor((decimal) perf.Audience / 5);
            return volumeCredits;
        }

        private static int CalculateAmountComedy(Performance perf)
        {
            int thisAmount = 30000;
            if (perf.Audience > 20)
            {
                thisAmount += 10000 + 500 * (perf.Audience - 20);
            }

            thisAmount += 300 * perf.Audience;
            return thisAmount;
        }

        private static int CalculateAmountTragedy(Performance perf)
        {
            int thisAmount = 40000;
            if (perf.Audience > 30)
            {
                thisAmount += 1000 * (perf.Audience - 30);
            }

            return thisAmount;
        }
    }
}
