.PHONY: build pathcp

build:
	dotnet publish -p:PublishSingleFile=true --self-contained false -r osx-x64 -o build

pathcp:
	sudo cp ./build/nlang /usr/local/bin/nlang