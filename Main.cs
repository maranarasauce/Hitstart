using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hitstart
{
    [BepInPlugin("maranara_hitstart", "Histart", "0.0.1")]
    public class Hitstart : BaseUnityPlugin
    {
        //Quick n dirty little mod. Not much organization - sorry!
        private void OnEnable()
        {
            Harmony harmony = new Harmony("hit_start");
            harmony.PatchAll(typeof(Hitstart));

            string[] arr = Environment.GetCommandLineArgs();
            foreach (string str in arr)
            {
                if (str.StartsWith("hitstop_"))
                {
                    if (float.TryParse(str.Remove(0, 8), out float result))
                    {
                        Debug.Log($"Hitstop modifier parsed as value {result}");
                        hitstopMult = result;
                        break;
                    }
                    else Debug.LogError("WARNING! YOUR HITSTOP CUSTOMIZATION DID NOT PARSE! USING DEFAULT OF 0");
                }
            }
        }

        public static float hitstopMult = 0f;
        [HarmonyPatch(typeof(TimeController), "HitStop")]
        [HarmonyPrefix]
        static void Hitstop(ref float length)
        {
            length = hitstopMult * length;
        }

        [HarmonyPatch(typeof(TimeController), "TrueStop")]
        [HarmonyPrefix]
        static void Truestop(ref float length)
        {
            length = hitstopMult * length;
        }

        [HarmonyPatch(typeof(TimeController), "SlowDown")]
        [HarmonyPrefix]
        static void Slowdown(ref float amount)
        {
            amount = hitstopMult * amount;
        }
    }
}
