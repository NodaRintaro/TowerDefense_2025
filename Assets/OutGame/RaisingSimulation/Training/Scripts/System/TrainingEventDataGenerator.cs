using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingEventDataGenerator
{
    /// <summary> CSVデータをTrainingEventClassに変換する処理 </summary>
    public static TrainingEventData GenerateEventData(string[] eventDataCSV)
    {
        TrainingEventData eventData = new TrainingEventData();

        eventData.Init(
            eventDataCSV[0],
            eventDataCSV[1],
            eventDataCSV[2],
            eventDataCSV[3],
            eventDataCSV[4],
            eventDataCSV[5],
            eventDataCSV[6],
            eventDataCSV[7],
            eventDataCSV[8],
            eventDataCSV[9],
            eventDataCSV[10],
            eventDataCSV[11],
            eventDataCSV[12]
            );

        return eventData;
    }
}
