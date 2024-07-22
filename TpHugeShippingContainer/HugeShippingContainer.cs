﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;

namespace TpHugeShippingContainer
{
	[HarmonyPatch]
	public class HugeShippingContainer
    {
		[HarmonyPrefix, HarmonyPatch(typeof(Game), nameof(Game.OnGameInstantiated))]
		public static void OnGameInstantiated_Prefix() {
			List<SourceThing.Row> list = EClass.sources.things.rows;
			FixContainerSize(list?.Find(x => x.name_JP == "出荷箱"), 16, 10);
		}

		[HarmonyPostfix, HarmonyPatch(typeof(Game), nameof(Game.OnGameInstantiated))]
		public static void OnGameInstantiated_Postfix() {
			var cs = EClass.game?.cards?.container_shipping?.trait as TraitShippingChest;
			cs?.owner?.things?.SetSize(16, 10);
		}

		private static void FixContainerSize(SourceThing.Row thing, int w, int h) {
			var traits = thing?.trait;
			if (traits == null) {
				return;
			}
			for (int i = 0; i < traits.Count(); i++) {
				try {
					if (Regex.Match(traits[i], @"^(Container.*)|(.*Chest)|(Fridge)$").Success
						&& (i + 2 < traits.Count())
					) {
						int wx = int.Parse(traits[i + 1]);
						int hx = int.Parse(traits[i + 2]);
						traits[i + 1] = (w).ToString();
						traits[i + 2] = (h).ToString();
						i += 2;
					}

				} catch (Exception) { }
			}
		}
	}
}
