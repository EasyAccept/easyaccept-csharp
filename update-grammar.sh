#!/usr/bin/env bash

./easyscript-antlr4/easy generate csharp --package EasyAccept.Core.Grammar --listener --visitor
mv ./easyscript-antlr4/.tmp/grammar/csharp/* ./EasyAccept.Core/Grammar/