# Github .NET SDK

This library is a sdk for accessing github's api using 
[FluentHttp](https://github.com/prabirshrestha/FluentHttp)

## Features

* supported HTTP methods - GET, POST, PUT, PATCH , DELETE and HEAD
* supported GitHub version - v2 and v3
* supported mode of authentications - HttpBasic and OAuth2

## Samples

### version 3

All github request returns code, headers. Body is returned for all HTTP methods except HEAD.

	var gh = new GithubApi(new GithubOAuthAuthenticator("access_token")); 
	dynamic result = gh.Get(GithubApiVersion.V3, "/user/followers");
	var httpStatusCode = result.code;
	var httpResponseHeaders = result.headers;
	var rateLimit = httpResponseHeaders["X-RateLimit-Limit"];
	var rateLimitRemaining =  httpResponseHeaders["X-RateLimit-Remaining"];
	dynamic response = result.body;

The body is a json object (IDicitionary&lt;string,object>) or json array (IList&lt;object>)

Example for PUT: (follow user)

	gh.Put(GithubApiVersion.V3, "/user/following/prabirshrestha");

Example for POST: (create key)

	var parameters = new Dictionary<string, object>();
    parameters["title"] = title;
    parameters["key"] = key;

    dynamic result = gh.Post(GithubApiVersion.V3, "/user/keys", parameters);

Example for PATCH: (update key)

    var parameters = new Dictionary<string, object>();
    if (!string.IsNullOrEmpty(title))
        parameters["title"] = title;
    if (!string.IsNullOrEmpty(key))
        parameters["key"] = key;

    if (parameters.Count == 0)
        throw new System.ArgumentException("Specify at least title or key");

    dynamic result = gh.Patch(GithubApiVersion.V3, string.Concat("/user/keys/", id), parameters);

Example for DELETE: (delete key)
	
	gh.Delete(GithubApiVersion.V3, string.Concat("/user/keys/", id));

### version 2 & downloading files

This example shows downloading files using github api version 2.

Like all api requests code, headers and body is returned. Body contains the byte[] which is the file.

    dynamic result = gh.Get(
        GithubApiVersion.V2, "/blob/show/defunkt/facebox/d4fc2d5e810d9b4bc1ce67702603080e3086a4ed");

	var httpStatusCode = result.code;
	var httpResponseHeaders = result.headers;
	var file = result.body;

    using (var fs = new FileStream("D:\\a.txt", FileMode.Create))
    {
        fs.Write(file, 0, file.Length);
    }

### Async sample

	gh.GetAsync(GithubApiVersion.V3, "/users/prabirshresth/gists", null, null,
		ar =>
		{
			if (ar.Exception != null)
				Console.WriteLine(ar.Exception);
			Console.WriteLine(ar.Result);
		});

## License
Apache License v2.0