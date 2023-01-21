# GrpcRestService

gRPC JSON transcoding creates RESTful JSON web APIs from gRPC methods. It uses annotations and options for customizing how a RESTful API maps to the gRPC methods.

## HTTP rules

gRPC methods must be annotated with an HTTP rule before they support transcoding. The HTTP rule includes information about calling the gRPC method as a RESTful API, such as the HTTP method and route.

## An HTTP rule is:

* An annotation on gRPC methods.
* Identified by the name google.api.http.
* Imported from the google/api/annotations.proto file. The google/api/http.proto and google/api/annotations.proto files need to be in the project.

## HTTP Method
The HTTP method is specified by setting the route to the matching HTTP method field name:

* get
* put
* post
* delete
* patch

```
service Address {
  rpc CreateAddress (CreateAddressRequest) returns (CreateAddressReply) {
    option (google.api.http) = {
      post: "/v1/address",
      body: "*"
    };
  }
}
```

## Route
gRPC JSON transcoding routes support route parameters. For example, {name} in a route binds to the name field on the request message.

To bind a field on a nested message, specify the path to the field. In the following example, {params.org} binds to the org field on the IssueParams message:

```
service Repository {
  rpc GetIssue (GetIssueRequest) returns (GetIssueReply) {
    option (google.api.http) = {
      get: "/{apiVersion}/{params.org}/{params.repo}/issue/{params.issueId}"
    };
  }
}

message GetIssueRequest {
  int32 api_version = 1;
  IssueParams params = 2;
}
message IssueParams {
  string org = 1;
  string repo = 2;
  int32 issueId = 3;
}
```

## Request body
Transcoding deserializes the request body JSON to the request message. The body field specifies how the HTTP request body maps to the request message. The value is either the name of the request field whose value is mapped to the HTTP request body or * for mapping all request fields.

In the following example, the HTTP request body is deserialized to the address field:

```
service Address {
  rpc AddAddress (AddAddressRequest) returns (AddAddressReply) {
    option (google.api.http) = {
      post: "/{apiVersion}/address",
      body: "address"
    };
  }
}

message AddAddressRequest {
  int32 api_version = 1;
  Address address = 2;
}
message Address {
  string street = 1;
  string city = 2;
  string country = 3;
}
```

## more information
https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-binding?view=aspnetcore-7.0

## to using gRPC JSON Transcoding
```
Microsoft.AspNetCore.Grpc.JsonTranscoding

builder.Services.AddGrpc().AddJsonTranscoding();
```

## gRPC JSON Transcoding | Swagger
```
Microsoft.AspNetCore.Grpc.Swagger

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(options =>
                               options.SwaggerDoc("v1", new OpenApiInfo
                               {
                                   Title = "gRPC Transcoding",
                                   Version = "v1"
                               }));

......

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

```
