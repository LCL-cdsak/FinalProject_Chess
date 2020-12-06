# Gameflow rule check 

名詞定義：
* 威脅路徑 - 從自身到王前一格，**並且自身位置=true**之bool map。
* thread - 在中間一敵方棋移開後，可直接攻擊國王(只有米字相關移動會有)。
* check - 可直接攻擊國王。
* all_team_path - 為普通吃棋bool map之集合(OR運算), 但友方位置為true(原本ValidPath遇到友方為false), 自身位置為false, 給敵對國王判定用。  


# Chess
chess中使用bool valid_left_castling, valid_right_castling用來記錄king rook移動情形，用做判定是否能長/短換位。

新增bool
* checking_king 若有任何敵方棋能夠"直接"威脅國王，為true。
在此暫無新增threading_king去判定是否有保王棋，直接看dictionary是否為空即可。

新增dictionary
* Dictionary<Piece, bool[8,8]> protect_king_pieces 儲存保王棋與敵方威脅路徑bool map。  
~~//* Dictionary<Piece, bool[8,8]> check_king_pieces 儲存非長直線威脅國王之piece極其威脅路徑(Pawn, Knight, King)。 ~~
~~//* Dictionary<Piece, bool[8,8]> path_check_king_pieces 儲存長直線威脅國王棋之piece及其威脅路徑(所有長直線移動之棋)。~~  
因為一回合內最多只有一個敵方棋能夠直接威脅國王，所以改用單變數儲存，也因AND運算方式，不須判別是否為長直線。  

新增bool map
* Dictionary<string, bool[8,8]> all_team_path - 為全部同team之OR運算結果(應在保王棋判斷完後再建立)。

1. 首先計算全部piece [thread_king_map]，並且將結果存至該保王棋之protect_path。
~~建立path_check_king_pieces, check_king_pieces.~~  
2. 計算piece之ValidPath，存至piece.valid_path。  
3. 將保國王piece.valid_path 與所有 thread_king_map(piece.protect_path) 做 **AND**。  
4. 建立所有隊伍之all_team_path。 
5. 將國王piece.valid_path(詳細於King Check)與敵方all_team_path做檢查[king.valid_path & !(all_team_path)]。
   * 若有威脅路徑無法被任何本team all_team_path阻擋，則需檢查國王是否能閃躲，若無法則敗。
   * 若可，則玩家必須幫國王阻擋。


# Piece
在Piece中新增"bool[8,8] valid_path"，會把結果存於此，chess中也直接對Piece.path_map做國王判定等操作。
新增bool  
* bool protecting_king; 即為保王棋。  
* bool can_protect_king; 即為潛在可保王棋，在check時，只能移動此項為true之棋。  
新增bool[]  
* protect_path - 由威脅棋計算，再添加到保王棋內。  

含有兩種valid path function。  

* ValidPath(old) 為到友軍停，到敵軍設成true後停。

* Thread_Cross/Diagonal_path 為到友軍停，到敵軍後再繼續偵測，若下一目標為國王，則回傳thread_king = true, 以及該威脅bool map。


# King Check
//國王被直接威脅時  
//1. 威脅棋不在九宮格內  
//   1. 長直線 - 確認是否有非保王棋可以擋。  
//   2. 非直線 - 利用all_team_path與國王valid_path做檢查(國王 & !(all_team_path))。  
//2. 威脅棋在九宮格內 - 直接用all_team_path, 國王valid_path做檢查(由於valid path不包含威脅棋本身，所以不影響王若可吃該棋的判斷)。  

國王走位計算  
利用AND敵方all_team_path計算，得到之結果則為可走路徑，若沒有任何，設king_cant_move為true。  
當check時，判斷king_cant_move，若無法，check是否有任何一個友方能夠走到威脅路徑上，沒有則敗(可能多設個bool於Piece，當check時判定是否為阻擋棋，若不是阻擋棋則不可動)。
