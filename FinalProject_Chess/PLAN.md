# Gameflow rule check 

名詞定義：
* 威脅路徑 - 從自身到王前一格，**並且自身位置=true**之bool map。
* thread - 在中間一敵方棋移開後，可直接攻擊國王(只有米字相關移動會有)。
* check - 可直接攻擊國王。


# Chess
chess中使用bool valid_left_castling, valid_right_castling用來記錄king rook移動情形，用做判定是否能長/短換位。

新增bool
* path_checking_king 若有任何敵方棋能夠"直接直線"威脅國王，為true。
在此暫無新增threading_king去判定是否有保王棋，直接看dictionary是否為空即可。

新增dictionary
* Dictionary<Piece, bool[8,8]> check_king_pieces 儲存非長直線威脅國王之piece極其威脅路徑(Pawn, Knight, King)。
* Dictionary<Piece, bool[8,8]> path_check_king_pieces 儲存長直線威脅國王棋之piece及其威脅路徑(所有長直線移動之棋)。
* Dictionary<Piece, bool[8,8]> protect_king_pieces 儲存保王棋與敵方威脅路徑bool map。  

新增bool map
* Dictionary<string, bool[8,8]> all_team_path - 為全部同team之OR運算結果(應在保王棋判斷完後再建立)。

1. 首先計算全部piece [path_map], [thread_king_map]，  
建立path_check_king_pieces
2. 將保國王piece.path_map 與所有 thread_king_map 做 **AND**。  
3. 建立all_team_path。
4. 將國王piece.path_map(詳細於King Check)。
   * 若有威脅路徑無法被任何本team all_team_path阻擋，則需檢查國王是否能閃躲，若無法則敗。
   * 若可，則玩家必須移動國王或是幫國王阻擋。


# Piece
在Piece中新增"bool[8,8] valid_path"，會把結果存於此，chess中也直接對Piece.path_map做國王判定等操作。

含有兩種valid path function。  

* ValidPath(old) 為到友軍停，到敵軍設成true後停。

* KingValidPath 為到友軍停，到敵軍後再繼續偵測，若下一目標為國王，則回傳thread_king = true, 以及該威脅bool map。


# King Check
國王被直接威脅時
1. 威脅棋不在九宮格內
   1. 長直線 - 確認是否有非保王棋可以擋。
   2. 非直線 - 利用all_team_path與國王valid_path做檢查(國王 & !(all_team_path))。
2. 威脅棋在九宮格內 - 直接用all_team_path, 國王valid_path做檢查(由於valid path不包含威脅棋本身，所以不影響王若可吃該棋的判斷)。
