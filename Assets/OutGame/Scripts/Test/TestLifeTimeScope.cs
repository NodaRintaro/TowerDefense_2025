using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class TestLIfeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        //基本的に登録する際はLifetime.Singleton
        builder.Register<TestInjectClassA>(Lifetime.Singleton);

        //TestInjectClassAが先に登録されているので
        //TestInjectClassBのコンストラクタの引数には上記で登録したInstanceが入る
        builder.Register<TestInjectClassB>(Lifetime.Singleton);
    }
}

//ロジッククラスA
public class TestInjectClassA
{
    private readonly int _testNumA;

    public int NumA => _testNumA;

    [Inject]
    public TestInjectClassA()
    {
        //初期化時に_testNumAの値を5にする
        _testNumA = 5;
    }

    public void Logger()
    {
        Debug.Log(_testNumA);
    }
}

//ロジッククラスB
public class TestInjectClassB
{
    private readonly int _testNumB;

    [Inject]
    public TestInjectClassB(TestInjectClassA testInjectClassA)
    {
        //先に登録されているTestInjectClassAクラスが引数に入るので_testNumBは5になる
        _testNumB = testInjectClassA.NumA;
    }

    public void Logger()
    {
        Debug.Log(_testNumB);
    }
}