using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TBot.Helpers;

namespace TBot.DataExtraction
{
    public class Bet
    {
        private int step;

        public string player1;
        public string player2;
        public string league;

        public DateTime date;

        public string type;
        public double bid;

        public string chances;
        public double predictedProfit;
        public double exchange;

        public Bet(Dictionary<string, string> betRawData)
        {
            try
            {
                this.step = betRawData["players"].Length % 3 + 2; //czy na pewno

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
                                this.league = decodeText(item.Value.Replace(ConfigHelper.webTemplate[item.Key], "").Trim());
                            } break;
                        case "type":
                            {
                                this.type = item.Value.Replace(ConfigHelper.webTemplate[item.Key], "").Trim();
                            } break;

                        case "date":
                            {
                                this.date = Convert.ToDateTime(item.Value);
                            } break;
                        case "bid":
                            {
                                this.bid = Convert.ToDouble(item.Value.Replace(ConfigHelper.webTemplate[item.Key], "")
                                                                     .Replace("j.", "")
                                                                     .Replace(".", ",")
                                                                     .Trim());
                            } break;
                        case "chancesMin":
                            {
                                this.chances = item.Value.Replace(ConfigHelper.webTemplate[item.Key], "").Trim();
                            } break;
                        case "predictedProfit":
                            {
                                this.predictedProfit = Convert.ToDouble(item.Value.Replace(ConfigHelper.webTemplate[item.Key], "")
                                                                                 .Replace("j.", "")
                                                                                 .Replace(".", ",")
                                                                                 .Trim());
                            } break;
                        case "exchange":
                            {
                                this.exchange = Convert.ToDouble(item.Value.Replace(ConfigHelper.webTemplate[item.Key], "")
                                                                           .Replace(".", ",")
                                                                           .Trim());
                            } break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception("Constructor(Bet) -> " + exc.Message);
            }
        }

        //private string getOnlyNumbers(string str)
        //{
        //    return Regex.Match(str, @"\d+").Value;
        //}

        private void getPlayers(string players)
        {
            try
            {
                string playersDecoded = decodeText(players);
                string[] playersDecodedTab = playersDecoded.Split('–');

                this.player1 = playersDecodedTab[0].Trim();
                this.player2 = playersDecodedTab[1].Trim();
            }
            catch (Exception exc)
            {
                throw new Exception("getPlayers -> " + exc.Message);
            }
        }

        private string decodeText(string text)
        {
            try
            {
                string textDecoded = "";

                for (int i = 0; i < text.Length; i++)
                {
                    if ((i + 1) % this.step == 0) continue;

                    textDecoded += text[i];
                }

                return textDecoded;
            }
            catch (Exception exc)
            {
                throw new Exception("decodeText -> " + exc.Message);
            }
        }
    }
}
