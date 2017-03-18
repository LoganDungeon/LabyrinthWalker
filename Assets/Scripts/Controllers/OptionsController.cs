using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptionsController {
    public static bool NewWorld = true;

    public static void PrintModus() {
        Debug.Log("Created New World: " + NewWorld);
    }
}
