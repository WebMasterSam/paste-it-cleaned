'use strict'

const fs = require('fs-extra')
const CodeGen = require('swagger-typescript-codegen').CodeGen

var file = 'swagger/swagger.json'
var swagger = JSON.parse(fs.readFileSync(file, 'UTF-8'))
var tsSourceCode = CodeGen.getTypescriptCode({
    className: 'BackendEntity',
    swagger: swagger,
})

fs.writeFileSync('src/entities/api.ts', tsSourceCode)
