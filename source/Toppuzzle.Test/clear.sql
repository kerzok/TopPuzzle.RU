﻿DELETE FROM [Picture]
DELETE FROM [Score]
DELETE FROM [Users]

DBCC CHECKIDENT ('[Picture]', RESEED, 0)
DBCC CHECKIDENT ('[Score]', RESEED, 0)
DBCC CHECKIDENT ('[Users]', RESEED, 0)