using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Linq;

namespace BadgerAOPVoteServer
{
    public class BadgerAOPVoteServer : BaseScript
    {
        string jsonConfig = LoadResourceFile(GetCurrentResourceName(), "config/config.json");

        bool badgerEssentialsLink;
        int aopVoteDuration;
        string colour1;
        string colour2;

        List<string> voteOptions = new List<string>();
        List<int> voteList = new List<int>();
        string voteOptionsMsg;
        int maxOptions;

        public BadgerAOPVoteServer()
        {
            Tick += OnTick;

            JObject o = JObject.Parse(jsonConfig);

            badgerEssentialsLink = (bool)o.SelectToken("badgerAOPVote.badgerEssentialsLink");
            colour1 = (string)o.SelectToken("badgerAOPVote.colour1");
            colour2 = (string)o.SelectToken("badgerAOPVote.colour2");

            EventHandlers["BadgerAOPVote:SendVoteToServer"] += new Action<int>(ReceiveVote);

            //
            // Commands
            //

            // Start an aop vote
            RegisterCommand("startVote", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // arg 0 = duration
                // arg 1+ = options
                voteOptions.Clear();
                voteList.Clear();

                object[] argArray = args.ToArray();
                maxOptions = args.Count - 1;
                voteOptionsMsg = string.Empty;

                int index = 0;
                foreach (string i in argArray)
                {
                    if (index > 0) // index 0 is for duration
                    {
                        voteOptions.Add(i);
                        voteOptionsMsg += colour1 + "Option " + index + ": " + colour2 + i + "\n";
                    }

                    index++;
                }

                aopVoteDuration = int.Parse(argArray[0].ToString());
                TriggerClientEvent("BadgerAOPVote:StartAOPVote", maxOptions, voteOptionsMsg, colour1, colour2, aopVoteDuration);
            }), false);

        }

        private async Task OnTick()
        {
            if (aopVoteDuration > 0)
            {
                await Delay(1000);
                aopVoteDuration--;
                if (aopVoteDuration <= 0)
                {
                    EndAOPVote();
                }
            }
        }

        private void SendAOPToBadgerEssentials(string aop)
        {
            TriggerEvent("BadgerEssentials:GetAOPFromBadgerAOPVote", aop);
        }

        private void ReceiveVote(int vote)
        {
            voteList.Add(vote);
        }

        private void EndAOPVote()
		{
            if (voteList.Count > 0)
			{
                int most = voteList.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                string aop = voteOptions[most - 1];

                TriggerClientEvent("BadgerAOPVote:EndVote", false, aop, colour1, colour2);

                if (badgerEssentialsLink)
                {
                    SendAOPToBadgerEssentials(aop);
                }
            }
            else // No Votes Received
			{

                TriggerClientEvent("BadgerAOPVote:EndVote", true, string.Empty, colour1, colour2);
            }
		}
    }
}
