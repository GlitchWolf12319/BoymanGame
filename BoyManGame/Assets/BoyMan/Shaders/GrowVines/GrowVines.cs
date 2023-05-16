{
    public List<MeshRenderer> growVinesMeshes;  // List of MeshRenderers for the growing vines
    public float timeToGrow = 5;  // Time it takes for the vines to fully grow
    public float refreshRate = 0.05f;  // Refresh rate for updating the growth progress
    [Range(0, 1)]
    public float minGrow = 0.2f;  // Minimum growth value for the vines
    [Range(0, 1)]
    public float maxGrow = 0.97f;  // Maximum growth value for the vines

    private List<Material> growVinesMaterials = new List<Material>();  // List of materials used for the vines
    private bool fullyGrown;  // Flag indicating if the vines are fully grown or not

    void Start()
    {
        // Iterate through each MeshRenderer
        for (int i = 0; i < growVinesMeshes.Count; i++)
        {
            // Iterate through each material of the MeshRenderer
            for (int j = 0; j < growVinesMeshes[i].materials.Length; j++)
            {
                if (growVinesMeshes[i].materials[j].HasProperty("Grow_"))
                {
                    // Set the initial growth value and add the material to the list
                    growVinesMeshes[i].materials[j].SetFloat("Grow_", minGrow);
                    growVinesMaterials.Add(growVinesMeshes[i].materials[j]);

                    // Start the coroutine to grow the vine using the material
                    StartCoroutine(GrowVine(growVinesMaterials[i]));
                }
            }
        }
    }

    IEnumerator GrowVine(Material mat)
    {
        float growValue = mat.GetFloat("Grow_");

        // Check if the vines are not fully grown
        if (!fullyGrown)
        {
            // Increase the growth value gradually until reaching the maximum growth value
            while (growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            // Decrease the growth value gradually until reaching the minimum growth value
            while (growValue > minGrow)
            {
                growValue -= 1 / (timeToGrow / refreshRate);
                mat.SetFloat("Grow_", growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }

        // Update the fullyGrown flag based on the final growth value
        if (growValue >= maxGrow)
        {
            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;
        }
    }
}