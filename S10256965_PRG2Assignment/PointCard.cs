//==========================================================
// Student Number : S10256965
// Student Name : Lee Jia Yu
// Partner Name : Yu Yi Lucas
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10256965_PRG2Assignment
{
    internal class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }
        public PointCard() { }
        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }
        public void AddPoints(int points) { }
        public void RedeemPoints(int points) { }
        public void Punch() { }
        public override string ToString()
        {
            return String.Format("Points: {0}, Punch Card: {1}, Tier: {2}", 
                                 Points, PunchCard, Tier);
        }
    }
}
