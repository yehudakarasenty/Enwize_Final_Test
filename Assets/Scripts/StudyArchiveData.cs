using Newtonsoft.Json;
using System.Collections.Generic;

//TODO: Delete File
public class StudyArchiveData 
{
    [JsonProperty("patients")]
    public List<StudyData> LocalStudies;
    [JsonProperty("procedures")]
    private List<StudyData> externalStudies;
    [JsonIgnore]
    public int x;
}

public class StudyData
{
}