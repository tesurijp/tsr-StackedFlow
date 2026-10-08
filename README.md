# tsr-StackedFlow

`tsr.StackedFlow` は、型付きのスタックをパイプラインとして扱うための小さな F# ライブラリです。  
値をスタックへ積み、スタック先頭から必要な個数の値を関数へ渡して、結果を再び積みます。

RPN 電卓のように、途中の値へ名前を付けずに入力・演算を連続して行える手軽さを、F# のパイプラインでも得ることを目的としています。  
パイプラインの各段階で、関数を呼び出せるだけの要素数があり、先頭の値の型が関数の引数型と合っているかを静的に確認できます。

Caller モジュールを利用することで、任意個の引数をスタックから取り出して実行することが出来ます。実行結果はスタックに積まれます。  
また、利用頻度の高いと思われる 1～3引数の関数の場合は、Callerモジュールを使わず  `apply1`〜`apply3`の引数として関数を与えることでシンプルに実行できます。
スタック用の特殊な関数を作成する必要はなく、通常のF#関数を指定することが出来るので、 既存の関数を組み合わせながら、名前付きの中間値を減らした処理を記述できます。

## できること

- スタックには任意の型の値を任意の順に積むことができます。
- 一般的な先頭要素の複製・削除・交換が可能です。
- スタックから任意個の引数を利用する関数を呼び出し、結果をスタックへ戻せます。
  - 対象とする関数はスタック専用に作られたものではなく、一般的な関数の使用が可能です。
  - スタック上の値の型、数などが呼び出そうとしている関数と一致しなければ、静的に型エラーとして検出されます。
- スタックの先頭だけを取り出すことで、通常の F# フローに戻れます。
- 頻繁に利用するであろう処理は、演算子による簡略化が可能です。  
  `push` に相当する `|+` と、applyN に相当する `|!`〜`|!!!` 演算子が用意されています。

## 使用例

```fsharp
open tsr.StackedFlow
open tsr.StackedFlow.Stack
open tsr.StackedFlow.StackOperator

let add3 x y z = x + y + z

let result =
    EmptyStack
    |+ 11
    |> dup
    |+ 10
    |> printStack     // 10::11::11
    |> dup
    |> printStack     // 10::10::11::11
    |> ternary add3    // 11 + 10 + 10
    |> printStack     // 31::11
    |> binary (*)     // 11 * 31
    |> printStack     // 341
```

スタックに対しては、引数の型が一致すればどのような F# 関数でも適用できます。

### 2種類の関数呼び出し方法

#### 3引数以下のための効率的な呼び出し

利用頻度が高いと思われる 3個以下の引数での関数呼び出しは、Caller モジュールを利用せず、直接呼び出せます。  

前述の使用例のように `unary`、 `binary`、`ternary` (`apply1`、`apply2`、`apply3`) または、演算子 `|!`、`|!!`、`|!!!` で3引数までの関数を呼び出せます。  
多くの用途では、こちらで対応が可能なはずです。

#### 汎用的な関数呼び出し

```fsharp
EmptyStack
    |+ "B+C "
    |+ 2
    |+ 3
    |+ "E + F - G "
    |+ 5
    |+ 6
    |+ 7
    |> printStack            // 7::6::5::E + F - G ::3::2::B+C
    |> Caller.prepareArgs5
    |> Caller.moreArgs2
    |> Caller.setFunc (fun a b c d e f g -> $"{a} = {b + c}  {d} = {e + f - g}" )
    |> Caller.callArgs2
    |> Caller.callArgs2
    |> Caller.callArgs2
    |> Caller.callArgs1
    |> Caller.finalize
    |> head
    |> printf "%s"         //  B+C  = 5  E + F - G  = 4
    |> ignore
```

4引数以上の任意の引数に対応する関数呼び出しは Caller モジュールを使い、関数呼び出しコンテキストを利用します。  

prepareArgsN によって関数呼び出し用コンテキストに移行し、不足する引数分だけ moreArgsN を実行し、引数を調整します。  
前述の例では、prepareArgsN によって5引数で初期化、moreArgs2 によって呼び出す引数を2増やし最終的に 7個に指定しています。

setFunc で呼び出す関数を指定します。

呼び出し時も同様に、callArgsN によって必要とするだけ引数を適用します。  
前述の例では callArgs2 を3回、callArgs1 を一回呼び出すことで 7引数に対応しています。(callArgs5 と callArgs2 を利用しても結果は同じです)  
moreArgsN と全く同じ組合せを指定する必要はありませんが、最終的な引数の数は一致させる必要があります。

全ての引数を適用したら、finalize によって値をスタックに書き戻します。  
全ての引数を提供していない場合 finalize は呼び出せません。

引数の数は1,2,5 ずつの、いずれ数単位で調整します。  
1～12個の引数は3回以下の操作で指定することが可能です。

## API

### tsr.StackedFlow 名前空間

tsr.StackedFlow 名前空間は、StackedFlow の定義全体を格納しています。  
直下には StackedFlow のパイプラインで利用される以下の型が定義されています。

| 型 | 説明 |
| --- | --- |
| `IStack` | EmptyStack、Stack<> の共通インターフェースです。 |
| `EmptyStack` | 空スタック |
| `Stack<'a, 'b>` | 'a は任意の型、'b は Stack<> または EmptyStackです。 |

### tsr.StackedFlow.Stack モジュール

tsr.StackedFlow.Stack モジュールは、一般的なスタックの操作と、利用頻度の高いであろう関数呼び出し機能が含まれます。

| 操作 | 説明 |
| --- | --- |
| `push value stack` | value を先頭へ積みます。 |
| `dup stack` | 先頭値を複製します。 |
| `over stack` | 2番目の値を複製します。 |
| `drop stack` | 先頭値を取り除きます。 |
| `swap stack` | 先頭の 2 値を交換します。 |
| `rot stack` | 先頭の 3 値をローテーションします。|
| `pop stack` | 先頭値と残りのスタックをタプルで返します。 |
| `head stack` | 先頭値を返します。残りのスタックは返しません。 |
| `unary func stack`<br>`(apply1 func stack)` | stack から1個の要素を取り出し func に適用し、結果をスタックに積みます。 |
| `binary func stack`<br>`(apply2 func stack)` | stack から2個の要素を取り出し func に適用し、結果をスタックに積みます。 |
| `ternary func stack`<br>`(apply3 func stack)` | stack から3個の要素を取り出し func に適用し、結果をスタックに積みます。 |
| `printStack stack` | スタックの内容を表示し、同じスタックを返します。デバッグ用にパイプラインへ挿入できます。 |

### tsr.StackedFlow.Caller モジュール

tsr.StackedFlow.Caller は汎用的な関数呼び出し機能が実装されています。  
一時的に、StackedFlow のパイプラインから Caller モジュールで定義するコンテキストのパイプラインに移行し、呼び出し完了後に元のパイプラインに復帰させます。

| 操作 | 説明 |
| --- | --- |
| `prepareArgs1 stack` | 引数1つの関数呼び出し用に、コンテキストを初期化します。 |
| `prepareArgs2 stack` | 引数2つの関数呼び出し用に、コンテキストを初期化します。 |
| `prepareArgs5 stack` | 引数5つの関数呼び出し用に、コンテキストを初期化します。 |
| `moreArgs1 precontext` | コンテキストが扱う引数を1 つ増やします。 |
| `moreArgs2 precontext` | コンテキストが扱う引数を2 つ増やします。 |
| `moreArgs5 precontext` | コンテキストが扱う引数を5 つ増やします。 |
| `setFunc func precontext` | コンテキストが呼び出す関数を指定します。 |
| `callArgs1 callcontext` | 1引数の関数を呼び出し、結果を保持します |
| `callArgs2 callcontext` | 2引数の関数を呼び出し、結果を保持します |
| `callArgs5 callcontext` | 5引数の関数を呼び出し、結果を保持します |
| `finalize callcontext` | 関数呼び出し結果を取り出し元のスタックフローに復帰します。 |

`prepareArgsN` を呼び出し後、`moreArgsN` を複数回呼び出すことで、任意個の引数に対応します。  
`setFunc` 呼び出し後は、`moreArgsN` を呼び出すことは出来ません。  
`callArgsN` は `setFunc` の後でなければ呼び出せません。`moreArgsN` と同様複数回呼び出すことで、任意個の引数に対応します。
(所定の引数に到達するまで、カリー化された引数 `setFunc`された状態となっています)  
`finalize` は、全ての引数が適用されるまで呼び出すことは出来ません。

### 演算子

tsr.StackedFlow.StackOperator に以下の演算子が定義されています。

| 演算子 | 対応関数  |
| --- | --- |
| `\|+` | `push` |
| `\|!` | `unary (apply1)` |
| `\|!!` | `binary (apply2)` |
| `\|!!!` | `ternary (apply3)` |

開発用のプロジェクト構成、Runner、テストの実行方法は [DEVELOPMENT.md](DEVELOPMENT.md) を参照してください。
