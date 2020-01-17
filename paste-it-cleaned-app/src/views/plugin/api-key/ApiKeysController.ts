import backend from '../../../config/backend.json'
import { BaseController } from '../../BaseController'

import { ConfigEntity, ApiKeyEntity } from '../../../entities/api'
import ApiKeys from './ApiKeys'

export class ApiKeysController extends BaseController {
    private component?: ApiKeys = undefined

    constructor(component: ApiKeys) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeApiKeys()
    }

    initializeApiKeys = () => {
        this.component!.setState({ apiKeysLoading: true })
        this.getApiKeysBackend(
            (apiKeys: ApiKeyEntity[]) => {
                this.component!.setState({ apiKeys, apiKeysLoading: false, isLoaded: true })
            },
            (e: any) => {
                this.component!.setState({ apiKeysLoading: false, apiKeysError: true, isLoaded: true })
            }
        )
    }

    createApiKey = () => {
        this.createApiKeyBackend(
            (apiKey: ApiKeyEntity) => {
                this.component!.setState({ apiKeyLoading: false })
                this.initializeApiKeys()
            },
            (e: any) => {
                this.component!.setState({ apiKeyLoading: false, apiKeyError: true })
            }
        )
    }

    private getApiKeysBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint(`/api-keys`), 'GET', success, error)
    }

    private deleteDomainBackend = (config: ConfigEntity, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys`), 'PUT', { config }, success, error)
    }

    private deleteApiKeyBackend = (config: ConfigEntity, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys`), 'PUT', { config }, success, error)
    }

    private saveApiKeyBackend = (config: ConfigEntity, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys`), 'PUT', { config }, success, error)
    }

    private createApiKeyBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys`), 'POST', {}, success, error)
    }

    private endpoint(path: string) {
        return this.culturedEndpoint(backend.endpoints.plugin + path)
    }
}
