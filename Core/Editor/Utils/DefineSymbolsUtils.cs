using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NabaGame.Core.Editor.Utils
{
    [InitializeOnLoad]
    public class DefineSymbolsUtils : MonoBehaviour
    {
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_ADMOB'", false, 1)]
        private static void NB_ADMOB()
        {
            NB_GlobalDefineUtils.AddDefine("NB_ADMOB");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'BB_APPLOVIN_MAX'", false, 2)]
        private static void SYMBOL_APPLOVIN_MAX()
        {
            NB_GlobalDefineUtils.AddDefine("BB_APPLOVIN_MAX");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'SYMBOL_AUDIENCE_NETWORK'", false, 3)]
        private static void SYMBOL_AUDIENCE_NETWORK()
        {
            NB_GlobalDefineUtils.AddDefine("SYMBOL_AUDIENCE_NETWORK");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_SINGULAR'", false, 4)]
        private static void NB_SINGULAR()
        {
            NB_GlobalDefineUtils.AddDefine("NB_SINGULAR");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_FIREBASE_ANALYTIC'", false, 5)]
        private static void NB_FIREBASE_ANALYTIC()
        {
            NB_GlobalDefineUtils.AddDefine("NB_FIREBASE_ANALYTIC");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_FACEBOOK_ANALYTIC'", false, 6)]
        private static void NB_FACEBOOK_ANALYTIC()
        {
            NB_GlobalDefineUtils.AddDefine("NB_FACEBOOK_ANALYTIC");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_BYTEBREW_ANALYTIC'", false, 7)]
        private static void NB_BYTEBREW_ANALYTIC()
        {
            NB_GlobalDefineUtils.AddDefine("NB_BYTEBREW_ANALYTIC");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_GAMEANALYTIC'", false, 8)]
        private static void NB_GAMEANALYTIC()
        {
            NB_GlobalDefineUtils.AddDefine("NB_GAMEANALYTIC");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'BB_FIREBASE_REMOTE'", false, 9)]
        private static void BB_FIREBASE_REMOTE()
        {
            NB_GlobalDefineUtils.AddDefine("BB_FIREBASE_REMOTE");
        }
        [MenuItem("Naba Game/Add Define Symbols/Add 'NB_APPFLYER'", false, 10)]
        private static void NB_APPFLYER()
        {
            NB_GlobalDefineUtils.AddDefine("NB_APPFLYER");
        }
        
        [MenuItem("Naba Game/Add Define Symbols/Add 'RELEASE'", false, 50)]
        private static void RELEASE()
        {
            NB_GlobalDefineUtils.AddDefine("RELEASE");
        }
        
        
        
        
        
        
        
        
         [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_ADMOB'", false, 1)]
        private static void RNB_ADMOB()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_ADMOB");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'BB_APPLOVIN_MAX'", false, 2)]
        private static void RSYMBOL_APPLOVIN_MAX()
        {
            NB_GlobalDefineUtils.RemoveDefine("BB_APPLOVIN_MAX");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'SYMBOL_AUDIENCE_NETWORK'", false, 3)]
        private static void RSYMBOL_AUDIENCE_NETWORK()
        {
            NB_GlobalDefineUtils.RemoveDefine("SYMBOL_AUDIENCE_NETWORK");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_SINGULAR'", false, 4)]
        private static void RNB_SINGULAR()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_SINGULAR");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_FIREBASE_ANALYTIC'", false, 5)]
        private static void RNB_FIREBASE_ANALYTIC()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_FIREBASE_ANALYTIC");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_FACEBOOK_ANALYTIC'", false, 6)]
        private static void RNB_FACEBOOK_ANALYTIC()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_FACEBOOK_ANALYTIC");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_BYTEBREW_ANALYTIC'", false, 7)]
        private static void RNB_BYTEBREW_ANALYTIC()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_BYTEBREW_ANALYTIC");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_GAMEANALYTIC'", false, 8)]
        private static void RNB_GAMEANALYTIC()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_GAMEANALYTIC");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'BB_FIREBASE_REMOTE'", false, 9)]
        private static void RBB_FIREBASE_REMOTE()
        {
            NB_GlobalDefineUtils.RemoveDefine("BB_FIREBASE_REMOTE");
        }
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'NB_APPFLYER'", false, 10)]
        private static void RNB_APPFLYER()
        {
            NB_GlobalDefineUtils.RemoveDefine("NB_APPFLYER");
        }
        
        [MenuItem("Naba Game/Remove Define Symbols/Remove 'RELEASE'", false, 50)]
        private static void RRELEASE()
        {
            NB_GlobalDefineUtils.RemoveDefine("RELEASE");
        }
        
        
        
    }
}