using System;

// a new class because reasons
public class MetricLogic {
    public double metricConversion(double num) {
        double meters = num * 0.0254;
        return meters;
    }
}