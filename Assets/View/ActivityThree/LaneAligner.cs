using UnityEngine;

public class LaneAligner : MonoBehaviour
{
    public RectTransform notesRoot;
    public PianoKey[] pianoKeys;
    public RectTransform[] lanes;

    void Start()
    {
        Align();
    }

    void Align()
    {
        if (pianoKeys.Length != lanes.Length)
        {
            Debug.LogError("❌ PianoKeys y Lanes deben tener el mismo tamaño");
            return;
        }

        for (int i = 0; i < pianoKeys.Length; i++)
        {
            RectTransform keyRect =
                pianoKeys[i].GetComponent<RectTransform>();

            RectTransform laneRect = lanes[i];

            // Posición de la tecla en mundo
            Vector3 worldPos = keyRect.position;

            // Convertir al espacio del NotesTimelineRoot
            Vector3 localPos =
                notesRoot.InverseTransformPoint(worldPos);

            // Solo copiamos X
            laneRect.anchoredPosition =
                new Vector2(localPos.x, laneRect.anchoredPosition.y);
        }
    }
}
