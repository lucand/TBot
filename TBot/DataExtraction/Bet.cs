using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Configuration;

namespace TBot.DataExtraction
{
    public class Bet
    {
        static NameValueCollection webTemplate = (NameValueCollection)ConfigurationManager.GetSection("webTemplate");

        int step;

        string player1;
        string player2;
        //asd
        string league;
        DateTime date;

        string type;
        decimal bid;
        string chances;
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
                            this.league = item.Value.Replace(webTemplate[item.Key].ToString(), "").Trim();
                        } break;
                    case "type":
                        {
                            this.type = item.Value.Replace(webTemplate[item.Key].ToString(), "").Trim();
                        } break;

                    case "date":
                        {
                            this.date = Convert.ToDateTime(betRawData["date"]);
                        } break;
                    case "bid":
                        {
                            this.bid = Convert.ToDecimal(item.Value.Replace(webTemplate[item.Key].ToString(), "")
                                                                 .Replace("j.", "")
                                                                 .Replace(".", "")
                                                                 .Trim());
                        } break;
                    case "chancesMin":
                        {
                            this.chances = item.Value.Replace(webTemplate[item.Key].ToString(), "").Trim();
                        } break;
                    case "predictedProfit":
                        {
                            this.predictedProfit = Convert.ToInt32(item.Value.Replace(webTemplate[item.Key].ToString(), "")
                                                                             .Replace("j.", "")
                                                                             .Trim());
                        } break;
                    case "exchange":
                        {
                            this.exchange = Convert.ToDouble(item.Value.Replace(webTemplate[item.Key].ToString(), "")
                                                                       .Replace(".", ",")
                                                                       .Trim());
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
            players = players.Trim();
            string playersDecoded = "";
            this.step = players.Length % 3 + 2; //czy na pewno

            for (int i = 0; i < players.Length; i++)
            {
                if ((i + 1) % step == 0) continue;

                playersDecoded += players[i];
            }
            string[] playersDecodedTab = playersDecoded.Split('-');
            this.player1 = playersDecodedTab[0].Trim();
            this.player2 = playersDecodedTab[1].Trim();
        }
    }
}
