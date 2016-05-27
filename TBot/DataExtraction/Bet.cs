using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TBot.DataExtraction
{
    public class Bet
    {
        string player1;
        string player2;
        //asd
        string league;
        DateTime date;

        string type;
        int bid;
        int chancesMin;
        int predictedProfit;
        double exchange;

        public Bet(Dictionary<string, string> betRawData)
        {
            foreach (var item in betRawData)
            {
                switch (item.Key)
                {
                    case "players":
                        {
                            getPlayers(item.Value);
                        } break;
                    case "league":
                        {
                            this.league = item.Value;
                        } break;
                    case "type":
                        {
                            this.type = item.Value;
                        } break;

                    case "date":
                        {
                            this.date = Convert.ToDateTime(betRawData["date"]);
                        } break;
                    case "bid":
                        {
                            this.bid = Convert.ToInt32(getOnlyNumbers(item.Value));
                        } break;
                    case "chancesMin":
                        {
                            this.chancesMin = Convert.ToInt32(getOnlyNumbers(item.Value));
                        } break;
                    case "predictedProfit":
                        {
                            this.predictedProfit = Convert.ToInt32(getOnlyNumbers(item.Value));
                        } break;
                    case "echange":
                        {
                            this.exchange = Convert.ToDouble(getOnlyNumbers(item.Value));
                        } break;
                }
            }
        }

        string getOnlyNumbers(string str)
        {
            return Regex.Match(str, @"\d+").Value;
        }

        void getPlayers(string players)
        {
        }
    }
}
