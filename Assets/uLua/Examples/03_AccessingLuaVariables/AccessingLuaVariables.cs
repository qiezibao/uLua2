﻿using LuaInterface;
using System.Text;
using UnityEngine;

public class AccessingLuaVariables : MonoBehaviour
{
	private string script = @"
			luanet.load_assembly('UnityEngine')
			GameObject = luanet.import_type('UnityEngine.GameObject')

			particles = {}

			for i = 1, Objs2Spawn, 1 do
				local newGameObj = GameObject('NewObj' .. tostring(i))
				local ps = newGameObj:AddComponent('ParticleSystem')
				ps:Stop()

				table.insert(particles, ps)
			end

			var2read = 42
		";

	void Start()
	{
		LuaState l = new LuaState();

		// Assign to global scope variables as if they're keys in a dictionary (they are really)
		l["Objs2Spawn"] = 5;
		l.DoString( Encoding.UTF8.GetBytes( script ) );

		// Read from the global scope the same way
		print( "Read from lua: " + l["var2read"].ToString() );

		// Get the lua table as LuaTable object
		LuaTable particles = (LuaTable)l["particles"];

		// Typical foreach over values in table
		foreach( ParticleSystem ps in particles.Values )
		{
			ps.Play();
		}
	}
}
