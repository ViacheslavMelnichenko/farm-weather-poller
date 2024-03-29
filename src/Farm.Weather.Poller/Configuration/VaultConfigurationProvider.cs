﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Farm.Weather.Poller.Configuration;

public class VaultConfigurationProvider : ConfigurationProvider
{
    private readonly string _hostingEnvironmentEnvironmentName;

    public VaultConfigurationProvider(string hostingEnvironmentEnvironmentName)
    {
        _hostingEnvironmentEnvironmentName = hostingEnvironmentEnvironmentName;
    }

    private const string VaultUrl = "VAULT_API_URL";
    private const string VaultToken = "VAULT_API_TOKEN";
    private const string Env = "prod";

    public override void Load()
    {
        if (_hostingEnvironmentEnvironmentName == "Development")
        {
            return;
        }

        Load(Environment.GetEnvironmentVariables());
    }

    private void Load(IDictionary envVariables)
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var vaultUrl = Environment.GetEnvironmentVariable(NormalizeKey(VaultUrl)) ??
                       throw new ArgumentNullException(VaultUrl,
                           $"{nameof(VaultToken)} environmental variable not found");

        var vaultToken = Environment.GetEnvironmentVariable(NormalizeKey(VaultToken)) ??
                         throw new ArgumentNullException(VaultToken,
                             $"{nameof(VaultUrl)} environmental variable not found");

        var vaultClientSettings =
            new VaultClientSettings(vaultUrl, new TokenAuthMethodInfo(vaultToken));
        var vaultClient = new VaultClient(vaultClientSettings);
        var path = $"{Env}/{typeof(Program).Assembly.GetName().Name}";
        var allKeyValues = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync<JsonObject>(path, mountPoint: "farm").Result.Data.Data;

        foreach (var keyValue in allKeyValues)
        {
            var key = keyValue.Key;
            var dictionary = keyValue.Value.Deserialize<IDictionary<string, string>>();

            foreach (var v in dictionary!)
            {
                data.Add($"{key}:{v.Key}", v.Value);
            }
        }

        foreach (var d in data)
        {
            Data.TryAdd(d.Key, d.Value);
        }
    }

    private static string NormalizeKey(string key) => key.Replace("__", ConfigurationPath.KeyDelimiter);
}