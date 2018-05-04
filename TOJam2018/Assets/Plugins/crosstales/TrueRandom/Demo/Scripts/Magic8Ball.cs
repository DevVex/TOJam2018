﻿using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.TrueRandom.Demo
{
    /// <summary>Magic 8-Ball simulator.</summary>
    [HelpURL("https://www.crosstales.com/media/data/assets/truerandom/api/class_crosstales_1_1_true_random_1_1_demo_1_1_magic8_ball.html")]
    public class Magic8Ball : MonoBehaviour {

        public Text Answer;

        private string[] answers = {
            // affirmative answers
            "It is certain",
            "It is decidedly so",
            "Without a doubt",
            "Yes definitely",
            "You may rely on it",
            "As I see it, yes",
            "Most likely",
            "Outlook good",
            "Yes",
            "Signs point to yes",
            // non-committal answers
            "Reply hazy try again",
            "Ask again later",
            "Better not tell you now",
            "Cannot predict now",
            "Concentrate and ask again",
            // negative answers
            "Don't count on it",
            "My reply is no",
            "My sources say no",
            "Outlook not so good",
            "Very doubtful"
        };

        public void Start() {
            if (Answer != null)
                Answer.text = string.Empty;
            
            TRManager.OnGenerateIntegerFinished += onGenerateIntegerFinished;
        }

        public void OnDestroy() {
            TRManager.OnGenerateIntegerFinished -= onGenerateIntegerFinished;
        }

        public void Ask() {
            TRManager.GenerateInteger (0, answers.Length - 1, 1);
        }

        private void onGenerateIntegerFinished (System.Collections.Generic.List<int> result, string id)
        {
            int index = result [0];
            
            if (index < 10) {
                if (Answer != null)
                    Answer.text = "<color=#00ff00ff>" + answers[index] + "</color>"; //green
            } else if (index > 14) {
                if (Answer != null)
                    Answer.text = "<color=#ff0000ff>" + answers[index] + "</color>"; //red
            } else {
                if (Answer != null)
                    Answer.text = "<color=#ffff00ff>" + answers[index] + "</color>"; //yellow
            }
        }
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)