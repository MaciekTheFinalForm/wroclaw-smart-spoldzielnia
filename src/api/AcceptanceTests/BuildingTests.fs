module BuildingTests

open HttpFs.Client
open Utils
open Hopac
open Newtonsoft.Json
open YoLo

//this assumes that api is already running!
let apiUrl = "http://localhost:5000/api" 

let getResource relativeUrl = Request.createUrl Get (apiUrl + relativeUrl) |> Request.responseAsString |> run

let settings = new JsonSerializerSettings(TypeNameHandling = TypeNameHandling.All)

[<CLIMutable>]
type BuildingRepresentation = 
  { Id : int; Name: string; Description : string }

[<Tests>]
let building = 
  "retrieve building" =>? [
    ( "should return error message for non existing resource" ->?
      let request = Request.createUrl Get (apiUrl + "/nonExisting") 
      let result = 
        job {
          use! response = getResponse request
          let code = response.statusCode
          let! body = Response.readBodyAsString response
          return code, (body |> String.trim)
        } 
        |> Job.toAsync 
        |> Async.RunSynchronously
      test <@ (404, "Status Code: 404; Not Found") = result @> )

    ( "should provide list of buildings" ->?
      let result = getResource "/buildings"
      let deserialized = JsonConvert.DeserializeObject<BuildingRepresentation array>(result)
      test <@ deserialized.Length > 1 @> )
  ]