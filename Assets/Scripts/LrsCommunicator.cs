using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;

//network clearance
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class LrsCommunicator : MonoBehaviour {

    [Header("Data Being Logged")]
    public string playerName = "";
    public string email = "";
    
    public int attempts;
    public bool wasSuccessful;
    public float score;
    
    public float arTimer;
    public float nonArTimer;
    
    public enum ViewType{other, AR, nonAR};
    public ViewType viewType;

    public bool lrsError = false;


    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if(viewType == ViewType.AR){
            arTimer += Time.deltaTime;
        }

        if(viewType == ViewType.nonAR){
            nonArTimer += Time.deltaTime;
        }
    }

    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for (int i=0; i<chain.ChainStatus.Length; i++) {
				if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}

    public void ReportScore(){        
        
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		var lrs = new RemoteLRS(
            "https://cloud.scorm.com/tc/B0MLOLLYBS/sandbox/",
            "alex.cha@mtsi-va.com",
            "Abcd1234"
        );
        
        var actor = new Agent();
        //actor.mbox = "mailto:info@tincanapi.com";
        actor.mbox = "mailto:" + email;
        actor.name = playerName;

        var verb = new Verb();
        verb.id = new Uri ("http://activitystrea.ms/schema/1.0/submit");
        verb.display = new LanguageMap();
        verb.display.Add("en-US", "submitted");

        var activity = new Activity();
        //id
        activity.id = "http://adlnet.gov/expapi/activities/assessment";
        
        //definition
        activity.definition = new ActivityDefinition();
            
            //definition name
        activity.definition.name = new LanguageMap();
        activity.definition.name.Add("en-US", "Assessment");

            //definition description
        activity.definition.description = new LanguageMap();
        activity.definition.description.Add("en-US", "A collection of questions");

            //definition type
        activity.definition.type = new Uri("http://adlnet.gov/expapi/activities/assessment");
        
        //result
        var result = new Result();
        result.completion = true;
        result.success = wasSuccessful;
        result.score = new Score();
        result.score.raw = score * 100;
        
        //declaring the full statement
        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;
        statement.result = result;


        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
        if (lrsResponse.success){
            // Updated 'statement' here, now with id
            Debug.Log("Score Reported: " + lrsResponse.content.id);
        }
        else {
            // Do something with failure
            Debug.Log("Failure to Report Score");
        }    
    }


    public void ReportArTime(){        
        
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		var lrs = new RemoteLRS(
            "https://cloud.scorm.com/tc/B0MLOLLYBS/sandbox/",
            "alex.cha@mtsi-va.com",
            "Abcd1234"
        );
        
        var actor = new Agent();
        //actor.mbox = "mailto:info@tincanapi.com";
        actor.mbox = "mailto:" + email;
        actor.name = playerName;

        var verb = new Verb();
        verb.id = new Uri ("http://id.tincanapi.com/verb/viewed");
        verb.display = new LanguageMap();
        verb.display.Add("en-US", "viewed");

        var activity = new Activity();
        //id
        activity.id = "http://adlnet.gov/expapi/activities/module";
        
        //definition
        activity.definition = new ActivityDefinition();
            
            //definition name
        activity.definition.name = new LanguageMap();
        activity.definition.name.Add("en-US", "Augmented Reality Missile");

            //definition description
        activity.definition.description = new LanguageMap();
        activity.definition.description.Add("en-US", "A view of the missile in augmented reality");

            //definition type
        activity.definition.type = new Uri("http://adlnet.gov/expapi/activities/module");
        
        //result
        var result = new Result();
        result.completion = true;
        result.success = wasSuccessful;
        result.score = new Score();
        result.score.raw = arTimer;
        
        //declaring the full statement
        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;
        statement.result = result;


        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
        if (lrsResponse.success){
            // Updated 'statement' here, now with id
            Debug.Log("AR Model Viewing Time Reported: " + lrsResponse.content.id);
        }
        else {
            // Do something with failure
            Debug.Log("Failure to Report Time Spent Viewing AR Model");
        }    
    }

    public void ReportNonArTime(){        
        
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		var lrs = new RemoteLRS(
            "https://cloud.scorm.com/tc/B0MLOLLYBS/sandbox/",
            "alex.cha@mtsi-va.com",
            "Abcd1234"
        );
        
        var actor = new Agent();
        //actor.mbox = "mailto:info@tincanapi.com";
        actor.mbox = "mailto:" + email;
        actor.name = playerName;

        var verb = new Verb();
        verb.id = new Uri ("http://id.tincanapi.com/verb/viewed");
        verb.display = new LanguageMap();
        verb.display.Add("en-US", "viewed");

        var activity = new Activity();
        //id
        activity.id = "http://adlnet.gov/expapi/activities/module";
        
        //definition
        activity.definition = new ActivityDefinition();
            
            //definition name
        activity.definition.name = new LanguageMap();
        activity.definition.name.Add("en-US", "3D Missile");

            //definition description
        activity.definition.description = new LanguageMap();
        activity.definition.description.Add("en-US", "A view of the missile in traditional 3D");

            //definition type
        activity.definition.type = new Uri("http://adlnet.gov/expapi/activities/module");
        
        //result
        var result = new Result();
        result.completion = true;
        result.success = wasSuccessful;
        result.score = new Score();
        result.score.raw = nonArTimer;
        
        //declaring the full statement
        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;
        statement.result = result;


        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
        if (lrsResponse.success){
            // Updated 'statement' here, now with id
            Debug.Log("3D Model Viewing Time Reported: " + lrsResponse.content.id);
        }
        else {
            // Do something with failure
            Debug.Log("Failure to Report 3D Model Viewing Time");
        }    
    }
    
    public void ReportLogIn(){        
        
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

		var lrs = new RemoteLRS(
            "https://cloud.scorm.com/tc/B0MLOLLYBS/sandbox/",
            "alex.cha@mtsi-va.com",
            "Abcd1234"
        );
        
        var actor = new Agent();
        //actor.mbox = "mailto:info@tincanapi.com";
        actor.mbox = "mailto:" + email;
        actor.name = playerName;

        var verb = new Verb();
        verb.id = new Uri ("https://brindlewaye.com/xAPITerms/verbs/loggedin/");
        verb.display = new LanguageMap();
        verb.display.Add("en-US", "Logged Into");

        var activity = new Activity();
        //id
        activity.id = "http://activitystrea.ms/schema/1.0/application";
        
        //definition
        activity.definition = new ActivityDefinition();
            
            //definition name
        activity.definition.name = new LanguageMap();
        activity.definition.name.Add("en-US", "Missile Training App");

            //definition description
        activity.definition.description = new LanguageMap();
        activity.definition.description.Add("en-US", "MTSI's Missile Training Application");

            //definition type
        activity.definition.type = new Uri("http://activitystrea.ms/schema/1.0/application");
        
        //declaring the full statement
        var statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;


        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
        if (lrsResponse.success){
            // Updated 'statement' here, now with id
            Debug.Log("Log In Reported: " + lrsResponse.content.id);
            lrsError = false;
        }
        else {
            // Do something with failure
            Debug.Log("Failed To Log In");
            lrsError = true;
        }    
    }

}