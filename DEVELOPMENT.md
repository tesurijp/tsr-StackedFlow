# 開発者向け情報

この文書はリポジトリを開発・検証するための情報です。NuGet パッケージの利用方法と API は [README.md](README.md) を参照してください。

## プロジェクト構成

| プロジェクト | 役割 |
| --- | --- |
| `src/StackedFlow` | `tsr.StackedFlow` ライブラリ本体です。 |
| `src/Runner.StackedFlow` | ライブラリ実装時に動作を確認するコンソール アプリケーションです。公開 API の一部ではありません。 |
| `test/Test.StakedFlow` | ライブラリ本体の自動テストです。|

## 実行とテスト

```powershell
# Runner を実行
dotnet run --project src/Runner.StackedFlow/Runner.StackedFlow.fsproj

# ライブラリのテストを実行
dotnet test test/Test.StakedFlow/Test.StakedFlow.fsproj

# ソリューション全体をテスト
dotnet test tsr-StackedFlow.slnx
```
