 
DROP PROCEDURE IF EXISTS GetStallBetLock;

DELIMITER //
  
CREATE OR REPLACE PROCEDURE GetStallBetLock 
(
  IN _ID_Agent bigint,
  IN _Date DATETIME
) 
LANGUAGE SQL
BEGIN  
 
 SELECT 
	REPLACE(CONCAT(pb.ID_Draw_Type, '-', pb.ID_Draw_Schedule , '-', pb.ID_Bet_Type, '-', pb.Digit_Combination, '-', pb.Amount), ' ', '') combet
	FROM Placed_Bet pb
	WHERE (pb.Datetime BETWEEN DATE_ADD(_Date, INTERVAL -1 MINUTE) AND DATE_ADD(_Date, INTERVAL 1 MINUTE)) AND pb.ID_Agent = _ID_Agent;
 
END;
//
DELIMITER ;