# Project Structure

## Form1.cs
只有GUI I/O, 例如用舊座標、新座標呼叫Chess class的function, Chess內判斷是否可執行, Form1再視情況更新畫面。

## Piece.cs
棋子Class，包含
* 棋子種類
* 可接收盤面上的狀態與自身座標，回傳可以走的路徑(bool[8,8])

PieceType為Enum，包含6個棋子種類，其隊伍使用string紀錄。

## Chess.cs
包含整個西洋棋規則，包含
* 棋盤狀態(Piece[8,8])，棋盤空格則為null
* 整個可進行的遊戲(初始化棋局、接收下棋指令、判斷勝負)
