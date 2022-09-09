using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordWobble : MonoBehaviour
{
    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;

    List<int> wordIndexes = new List<int> { 0 };
    List<int> wordLengths = new List<int>();

    private bool isWobble = false;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();

        mesh = textMesh.mesh;
    }

    public void SetWobble(string text)
    {
        isWobble = true;
        textMesh.text = text;
        wordIndexes.Clear();
        wordIndexes.Add(0);
        wordLengths.Clear();

        string s = textMesh.text;
        for (int index = s.IndexOf(' '); index > -1; index = s.IndexOf(' ', index + 1))
        {
            wordLengths.Add(index - wordIndexes[wordIndexes.Count - 1]);
            wordIndexes.Add(index + 1);
        }
        wordLengths.Add(s.Length - wordIndexes[wordIndexes.Count - 1]);
    }
    
    public void StopWobble()
    {
        isWobble = false;
        textMesh.ForceMeshUpdate();
    }

    private void Update()
    {
        if (isWobble)
        {
            textMesh.ForceMeshUpdate();
            vertices = mesh.vertices;

            for (int w = 0; w < wordIndexes.Count; w++)
            {
                int wordIndex = wordIndexes[w];
                Vector3 offset = Wobble(Time.time + w);

                for (int i = 0; i < wordLengths[w]; i++)
                {
                    TMP_CharacterInfo c = textMesh.textInfo.characterInfo[wordIndex + i];

                    int index = c.vertexIndex;

                    vertices[index] += offset;
                    vertices[index + 1] += offset;
                    vertices[index + 2] += offset;
                    vertices[index + 3] += offset;
                }
            }

            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 10f) * 5, Mathf.Cos(time * 10f) * 2);
    }
}