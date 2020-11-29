using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using System.Linq;

namespace BadgerAOPVote
{
    public class BadgerAOPVoteClient : BaseScript
    {
        bool aopVoteActive = false;
        bool canVote = false;
        int maxOptions = 0;

        public BadgerAOPVoteClient()
		{

            EventHandlers["BadgerAOPVote:StartAOPVote"] += new Action<int, string, string, string, int>(StartVote);
            EventHandlers["BadgerAOPVote:EndVote"] += new Action<bool, string, string, string>(EndVote);

            //
            // Commands
            //

            // Vote for an aop
            RegisterCommand("vote", new Action<int, List<object>, string>((source, args, raw) =>
            {
                Debug.WriteLine("vote command");

                // If there is an active vote
                if (aopVoteActive)
                {
                    if (canVote)
                    {
                        int vote = int.Parse(args[0].ToString());

                        // If vote is invalid
                        if (vote > maxOptions || vote <= 0)
                        {
                            Screen.ShowNotification("There is no option " + vote);
                        }
                        else
                        {
                            TriggerServerEvent("BadgerAOPVote:SendVoteToServer", vote);
                            canVote = false;
                            Screen.ShowNotification("Your vote has been recorded");
                        }
                    }
                    else
                    {
                        Screen.ShowNotification("You have already voted!");
                    }
                }
                else 
                {
                    Screen.ShowNotification("No AOP Vote is currently active");
                }
            }), false);
        }

        private void StartVote(int maxOption, string options, string colour1, string colour2, int timeToVote)
		{
            maxOptions = maxOption;
            aopVoteActive = true;
            canVote = true;

            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                multiline = true,
                args = new[] { colour2 + "An " + colour1 + "AOP Vote " + colour2 + "has been started! " + 
                "You have " + colour1 + timeToVote + " seconds " + colour2 + "to vote using " + colour1 +  "/vote <option> " + "\n" +
                options }
            });
        }

        private void EndVote(bool noVotes, string aop, string colour1, string colour2)
		{
            canVote = false;
            aopVoteActive = false;

            if (noVotes)
            {
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    multiline = true,
                    args = new[] { colour1 + "AOP Vote " + colour2 + "has ended without any votes" }
                });
            }
            else
            {
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    multiline = true,
                    args = new[] { colour1 + "AOP Vote " + colour2 + "has ended! " + "The winning aop is " + colour1 + aop }
                });
            }
        }
    }
}
