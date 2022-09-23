const vscode = require('vscode');
const fs = require('fs');

/**
 * @param {vscode.ExtensionContext} context
 */
function activate(context) {
	let disposable = vscode.commands.registerCommand('nlang-language-support.helloWorld', function () {
		vscode.window.showInformationMessage('Hello World!');
	});

	context.subscriptions.push(disposable);

	let completionProvider = vscode.languages.registerCompletionItemProvider('nlang', {
		provideCompletionItems(document, position, token, context) {
			let completionItems = [];

			let types = ['int', 'float', 'string', 'bool', 'void', 'byte', 'char', 'short', 'long', 'double'];
			let funcs = [
				'for', 'while', 'if', 'else', 'switch', 'case', 'default', 'break', 'continue', 'return', 'goto',
				'printf', 'print', 'foreach', 'in', 'import', 'cimport', 'class', 'struct', 'enum'
			];

			for (let i = 0; i < types.length; i++) {
				let item = new vscode.CompletionItem(types[i]);
				item.kind = vscode.CompletionItemKind.Keyword;
				completionItems.push(item);
			}

			for (let i = 0; i < funcs.length; i++) {
				let item = new vscode.CompletionItem(funcs[i]);
				item.kind = vscode.CompletionItemKind.Function;
				completionItems.push(item);
			}

			// If after import, send file suggestions
			let line = document.lineAt(position.line).text;
			let cur = line.substring(0, position.character);
			let curWord = cur.split(' ')[cur.split(' ').length - 1];
			if (curWord.includes('/')) {
				let dir = curWord.substring(0, curWord.lastIndexOf('/'));
				let files = fs.readdirSync(dir);
				for (let i = 0; i < files.length; i++) {
					let item = new vscode.CompletionItem(files[i]);
					item.kind = vscode.CompletionItemKind.File;
					completionItems.push(item);
				}
			}

			let files = fs.readdirSync(vscode.workspace.rootPath);
			for (let i = 0; i < files.length; i++) {
				let item = new vscode.CompletionItem(files[i]);
				item.kind = vscode.CompletionItemKind.File;
				completionItems.push(item);
			}


			let completionItem = new vscode.CompletionItem('Hello World');
			completionItem.insertText = 'Hello World';
			completionItems.push(completionItem);

			return completionItems;
		}
	});

	context.subscriptions.push(completionProvider);

	// Semantic highlighting:
	let semanticProvider = vscode.languages.registerDocumentSemanticTokensProvider('nlang', {
		provideDocumentSemanticTokens(document, token) {
			let tokens = [];

			let types = ['int', 'float', 'string', 'bool', 'void', 'byte', 'char', 'short', 'long', 'double'];
			let funcs = [
				'for', 'while', 'if', 'else', 'switch', 'case', 'default', 'break', 'continue', 'return', 'goto',
				'printf', 'print', 'foreach', 'in', 'import', 'cimport', 'class', 'struct', 'enum'
			];

			for (let i = 0; i < document.lineCount; i++) {
				let line = document.lineAt(i).text;
				let words = line.split(' ');
				for (let j = 0; j < words.length; j++) {
					let word = words[j];
					if (types.includes(word)) {
						tokens.push(new vscode.SemanticToken(i, j, 1, 0));
					} else if (funcs.includes(word)) {
						tokens.push(new vscode.SemanticToken(i, j, 2, 0));
					}
				}
			}

			return new vscode.SemanticTokens(tokens);
		}
	}, new vscode.SemanticTokensLegend(['type', 'function']));
	context.subscriptions.push(semanticProvider);
}

// this method is called when your extension is deactivated
function deactivate() {}

module.exports = {
	activate,
	deactivate
}
