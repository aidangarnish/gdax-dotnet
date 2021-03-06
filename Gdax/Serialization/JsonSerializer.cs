﻿using System;
using Newtonsoft.Json;

namespace Gdax
{
	internal class JsonSerializer : ISerialzer
	{
		public String Serialize<T>(T value)
		{
			return JsonConvert.SerializeObject(value);
		}

		public T Deserialize<T>(String value)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(value);
			}
			catch (JsonSerializationException ex)
			{
				GdaxErrorMessage error = null;

				try
				{
					error = JsonConvert.DeserializeObject<GdaxErrorMessage>(value);
				}
				catch (Exception)
				{
					// NOOP
				}

				if (error != null)
				{
					throw new GdaxException(error.Message);
				}
				else
				{
					throw new GdaxException($"Could not deserialize '{value}'.", ex);
				}
			}
		}
		
		private class GdaxErrorMessage
		{
			[JsonProperty("message")]
			public String Message { get; set; }
		}
	}
}
