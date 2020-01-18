import backend from '../../../config/backend.json'
import { BaseController } from '../../BaseController'
import { ConfigEntity, ApiKeyEntity, DomainEntity } from '../../../entities/api'
import ApiKeys from './ApiKeys'
import moment from 'moment'

export class ApiKeysController extends BaseController {
    private component?: ApiKeys = undefined

    constructor(component: ApiKeys) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeApiKeys()
    }

    initializeApiKeys = (callback?: () => void) => {
        this.component!.setState({ apiKeysLoading: true })
        this.getApiKeysBackend(
            (apiKeys: ApiKeyEntity[]) => {
                this.component!.setState({ apiKeys, apiKeysLoading: false, isLoaded: true })
                callback && callback()
            },
            (e: any) => {
                this.component!.setState({ apiKeysLoading: false, apiKeysError: true, isLoaded: true })
            }
        )
    }

    refreshApiKeys = (callback?: () => void) => {
        this.getApiKeysBackend(
            (apiKeys: ApiKeyEntity[]) => {
                this.component!.setState({ apiKeys })
                callback && callback()
            },
            (e: any) => {
                this.component!.setState({ apiKeysError: true })
            }
        )
    }

    createApiKey = () => {
        this.createApiKeyBackend(
            (apiKey: ApiKeyEntity) => {
                this.refreshApiKeys(() => this.showSuccessSnackbar('Api key generated successfuly.'))
            },
            (e: any) => {
                this.showErrorSnackbar('An error occured while generating the api key.')
            }
        )
    }

    deleteApiKey = (apiKeyId: string) => {
        this.deleteApiKeyBackend(
            apiKeyId,
            () => {
                this.refreshApiKeys(() => this.showSuccessSnackbar('Api key deleted successfuly.'))
            },
            (e: any) => {
                this.showErrorSnackbar('An error occured while deleting the api key.')
            }
        )
    }

    deleteDomain = (apiKeyId: string, domainId: string) => {
        this.deleteDomainBackend(
            apiKeyId,
            domainId,
            () => {
                this.refreshApiKeys(() => this.showSuccessSnackbar('Domain deleted successfuly.'))
            },
            (e: any) => {
                this.showErrorSnackbar('An error occured while deleting the domain.')
            }
        )
    }

    createDomain = (apiKeyId: string, domainName: string) => {
        this.createDomainBackend(
            apiKeyId,
            domainName,
            (domain: DomainEntity) => {
                this.refreshApiKeys(() => this.showSuccessSnackbar('Domain added successfuly.'))
            },
            (e: any) => {
                this.showErrorSnackbar('An error occured while adding the domain.')
            }
        )
    }

    updateApiKey = (key: ApiKeyEntity) => {
        this.updateApiKeyBackend(
            key,
            (domain: DomainEntity) => {
                this.refreshApiKeys(() => this.showSuccessSnackbar('Api key updated successfuly.'))
            },
            (e: any) => {
                this.showErrorSnackbar('An error occured while updating the api key.')
            }
        )
    }

    showAddDomain = (key: ApiKeyEntity) => {
        this.component!.setState({
            modalAddDomain: {
                visible: true,
                keyEntity: key,
                domainName: '',
                error: false,
            },
        })
    }

    handleAddDomainTyping = (e: any) => {
        const newValue = e.target.value
            .toLowerCase()
            .trim()
            .replace(' ', '')
            .replace('www.', '')
            .replace('%20', '')
            .replace(/.+\:\/\//, '')
            .split('/')[0]
            .split(':')[0]
        const domainRegex = /^[a-z0-9]+(\.[a-z0-9]+)*\.([a-z]{1,20}|[0-9]{1,3})$/
        const error = !domainRegex.test(newValue)

        this.component!.setState({
            modalAddDomain: {
                ...this.component!.state.modalAddDomain,
                domainName: newValue,
                error,
            },
        })
    }

    hideAddDomain = () => {
        this.component!.setState({
            modalAddDomain: {
                ...this.component!.state.modalAddDomain,
                visible: false,
            },
        })
    }

    showDeleteDomain = (key: ApiKeyEntity, domain: DomainEntity) => {
        this.component!.setState({
            modalDeleteDomain: {
                visible: true,
                keyEntity: key,
                domainEntity: domain,
            },
        })
    }

    hideDeleteDomain = () => {
        this.component!.setState({
            modalDeleteDomain: {
                ...this.component!.state.modalDeleteDomain,
                visible: false,
            },
        })
    }

    showDeleteApiKey = (key: ApiKeyEntity) => {
        this.component!.setState({
            modalDeleteApiKey: {
                visible: true,
                keyEntity: key,
            },
        })
    }

    hideDeleteApiKey = () => {
        this.component!.setState({
            modalDeleteApiKey: {
                ...this.component!.state.modalDeleteApiKey,
                visible: false,
            },
        })
    }

    showUpdateApiKey = (key: ApiKeyEntity) => {
        this.component!.setState({
            modalUpdateApiKey: {
                visible: true,
                keyEntity: key,
            },
        })
    }

    handleUpdateApiKeyExpiresOn = (date: Date | null) => {
        this.component!.setState({
            modalUpdateApiKey: {
                ...this.component!.state.modalUpdateApiKey,
                keyEntity: {
                    ...this.component!.state.modalUpdateApiKey.keyEntity!,
                    expiresOn: (date ? moment(date) : moment()).toISOString(),
                },
            },
        })
    }

    hideUpdateApiKey = () => {
        this.component!.setState({
            modalUpdateApiKey: {
                ...this.component!.state.modalUpdateApiKey,
                visible: false,
            },
        })
    }

    showErrorSnackbar = (message: string) => {
        this.component!.setState({
            errorMessage: {
                visible: true,
                message,
            },
        })
    }

    hideErrorSnackbar = () => {
        this.component!.setState({
            errorMessage: {
                ...this.component!.state.errorMessage,
                visible: false,
            },
        })
    }

    showSuccessSnackbar = (message: string) => {
        this.component!.setState({
            successMessage: {
                visible: true,
                message,
            },
        })
    }

    hideSuccessSnackbar = () => {
        this.component!.setState({
            successMessage: {
                ...this.component!.state.successMessage,
                visible: false,
            },
        })
    }

    private getApiKeysBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint(`/api-keys`), 'GET', success, error)
    }

    private createApiKeyBackend = (success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys`), 'POST', {}, success, error)
    }

    private createDomainBackend = (apiKeyId: string, domainName: string, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys/${apiKeyId}/domains`), 'POST', { domainName }, success, error)
    }

    private deleteDomainBackend = (apiKeyId: string, domainId: string, success?: () => void, error?: (e: any) => void) => {
        this.callBackendWithoutResponse(this.endpoint(`/api-keys/${apiKeyId}/domains/${domainId}`), 'DELETE', success, error)
    }

    private deleteApiKeyBackend = (apiKeyId: string, success?: () => void, error?: (e: any) => void) => {
        this.callBackendWithoutResponse(this.endpoint(`/api-keys/${apiKeyId}`), 'DELETE', success, error)
    }

    private updateApiKeyBackend = (apiKey: ApiKeyEntity, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackendWithPayload(this.endpoint(`/api-keys/${apiKey.apiKeyId!}`), 'PUT', { apiKey }, success, error)
    }

    private endpoint(path: string) {
        return this.culturedEndpoint(backend.endpoints.plugin + path)
    }
}
